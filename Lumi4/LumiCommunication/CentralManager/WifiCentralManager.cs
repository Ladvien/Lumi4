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
using Lumi4.DeviceState;
using Lumi4.LumiCommunication.PeripheralManager;
namespace Lumi4.LumiCommunication.CentralManager
{
    public class WifiCentralManager: Central
    {
        #region properties
        private Uri IP { get; set; }

        public string WebServerUrl { get; private set; }
        HttpClient httpClient = new HttpClient();

        #endregion

        public WifiCentralManager(Uri ip)
        {
            if (ip == null) throw new ArgumentNullException();
            else IP = ip;
        }

        public override void Start()
        {
            UpdateDeviceStateWithWifiStatus();
        }

        public async void Search(int startIndex, int endIndex, int timeout = 300, ProgressBar progressBar = null)
        {

            // 1. Convert the passed IP down to 3 places.
            // 2. Create System.Net.Http.HttpClient (NOTE: Windows HttpClient doesn't seem to have 
            //    adjustable timeout.)
            // 3. Set the POST string to "name".  This will be used as a handshake.
            // 4. Iterate over the IP range, POSTing handshake.
            // 5. If handshake successful at IP, create a peripheral and add it to list.
            // 6. Update DeviceState: Searching->On
            // 7. After iteration, return list, even if empty.
            
            DataConversion dataConverter = new DataConversion();
            var threePartIP = DataConversion.seperateStringByCharacterIndex(IP.ToString(), 3, '.');
            var httpClient = new System.Net.Http.HttpClient();
            httpClient.Timeout = new TimeSpan(0, 0, 0, 0, timeout);

            if (progressBar != null)
            {
                progressBar.Maximum = endIndex - startIndex;
            }

            DeviceState.State = States.Searching;
            OnDeviceStateChange(DeviceState);

            for (int i = startIndex; i < endIndex; i++)
            {
                try
                {
                    string ip = threePartIP + i.ToString() + "/";
                    var resourceUri = new Uri(ip + HttpPeripheral.WebServiceGetName);
                    var response = await httpClient.PostAsync(resourceUri, null);
                    if (response.IsSuccessStatusCode == true)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        if (responseString != "")
                        {
                            try
                            {
                                HttpPeripheral peripheral = new HttpPeripheral(responseString, new Uri(ip));
                                OnDiscoveringDevice(peripheral);
                            } catch (Exception ex)
                            {

                            }

                        }
                    }
                    response.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in WifiCentralManager.Search: " + ex.Message);
                }
                if(progressBar != null) { progressBar.Value += 1; }   
            }
            if(progressBar != null)
            {
                progressBar.Value = 0;
                progressBar.IsEnabled = false;
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
                var httpPeripheral = peripheral as HttpPeripheral;
                Uri ip = httpPeripheral.PeripheralInfo.IP;
                var resourceUri = new Uri(ip + HttpPeripheral.WebServiceConnect);
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
    }

}

