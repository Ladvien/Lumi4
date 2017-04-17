using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Radios;
using Lumi4.LumiCommunication.DataHandling;
using System.Net.Http;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Lumi4.DeviceState;
using Lumi4.LumiCommunication.PeripheralManager;
using System.Net;

namespace Lumi4.LumiCommunication.CentralManager
{
    public class WebServerCentralManager: Central
    {
        #region properties
        private Uri IP { get; set; }

        public string WebServerUrl { get; private set; }
        HttpClient httpClient = new HttpClient();
        private Dictionary<string, WebServerPeripheral> DiscoveredPeripherals = new Dictionary<string, WebServerPeripheral>();

        #endregion

        public WebServerCentralManager(string ip = "http://192.168.1.1/")
        {
            
            try
            {
                Uri IP = new Uri(ip);
                SetApproximateNetwork(IP);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in WebServerCentralManager: " + ex.Message);
            }
        }

        public void SetApproximateNetwork(Uri ip)
        {
            if (ip == null) throw new ArgumentNullException();
            else IP = ip;
        }

        public override void Start()
        {
            UpdateDeviceStateWithWifiStatus();
        }

        public async void Search(int startIndex, int endIndex, int timeout = 300)
        {

            // 1. Convert the passed IP down to 3 places.
            // 2. Create System.Net.Http.HttpClient (NOTE: Windows HttpClient doesn't seem to have 
            //    adjustable timeout.)
            // 3. Set the POST string to "name".  This will be used as a handshake.
            // 4. Iterate over the IP range, POSTing handshake.
            // 5. If handshake successful at IP, create a peripheral and add it to list.
            // 6. Update DeviceState: Searching->On
            // 7. After iteration, return list, even if empty.

            DiscoveredPeripherals = new Dictionary<string, WebServerPeripheral>();
        
            DataConversion dataConverter = new DataConversion();
            var fourthPartOfIp = DataConversion.SeperateStringByCharacterIndex(IP.ToString(), 3, '.');
            var threePartIP = IP.ToString().Replace(fourthPartOfIp, "");
            var httpClient = new System.Net.Http.HttpClient();
            httpClient.Timeout = new TimeSpan(0, 0, 0, 0, timeout);

            DeviceState.State = States.Searching;
            OnDeviceStateChange(DeviceState);

            for (int i = startIndex; i < endIndex; i++)
            {
                try
                {
                    string ip = threePartIP + i.ToString() + "/";
                    var resourceUri = new Uri(ip + WebServerPeripheral.WebServiceGetName);
                    var response = await httpClient.PostAsync(resourceUri, null);
                    if (response.IsSuccessStatusCode == true)
                    {
                        var deviceName = await response.Content.ReadAsStringAsync();
                        if (deviceName != "")
                        {
                            try
                            {
                                WebServerPeripheral peripheral = new WebServerPeripheral(deviceName, new Uri(ip));
                                DiscoveredPeripherals.Add(deviceName, peripheral);
                                OnDiscoveringDevice(peripheral);
                            } catch (Exception ex)
                            {
                                Debug.WriteLine("Exception in WifiCentralManager.Search: " + ex.Message);
                            }
                        }
                    }
                    response.Dispose();
                }
                catch (Exception ex)
                {
                    OnDiscoveringDevice(null);
                    Debug.WriteLine("Exception in WifiCentralManager.Search: " + ex.Message);
                } 
            }
            UpdateDeviceStateWithWifiStatus();
        }

        private async void UpdateDeviceStateWithWifiStatus()
        {
            var wifiOn = await IsWifiOn();
            if (wifiOn) { DeviceState.State = States.On; } else { DeviceState.State = States.Off; }
            OnDeviceStateChange(DeviceState);
        }

        public async Task<bool> IsWifiOn()
        {
            await Radio.RequestAccessAsync();
            var radios = await Radio.GetRadiosAsync();
            foreach (var radio in radios)
            {
                if (radio.Kind == RadioKind.WiFi)
                {
                    return radio.State == RadioState.On;
                }
            }
            return false;
        }

        override async public Task<bool> Connect(Peripheral peripheral)
        {
            try
            {
                var httpPeripheral = peripheral as WebServerPeripheral;
                Uri ip = httpPeripheral.PeripheralInfo.IP;
                var resourceUri = new Uri(ip + WebServerPeripheral.WebServiceConnect);
                var response = await httpClient.PostAsync(resourceUri, null);
            } catch (Exception ex)
            {
                Debug.WriteLine("Exception in WiFiCentralManager: " + ex.Message);
                return false;
            }
            
            return true;
        }

        public override async Task<bool> Disconnect(Peripheral peripheral)
        {
            throw new NotImplementedException();
            return true;
        }

        public WebServerPeripheral GetDiscoveredPeripheralByName(string name)
        {
            return DiscoveredPeripherals[name];
        }

    }

}

