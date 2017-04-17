using Lumi4.LumiCommunication.CentralManager;
using Lumi4.LumiCommunication.DataHandling;
using Lumi4.LumiCommunication.PeripheralManager;
using Lumi4.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lumi4.Lumi4App.Models
{
    public class Lumi4Model
    {
        private WebServerCentralManager WebServerCentralManager;
        private ProgressBar progressBar = new ProgressBar();

        public Lumi4Model(WebServerCentralManager _webServerCentralManager)
        {
            WebServerCentralManager = _webServerCentralManager; 
            InitializeSettings();
        }

        public void InitializeSettings()
        {
            byte[] testPacket = { 0x48, 0x45, 0x59, 0x20, 0x59, 0x4F, 0x55 };
            WebServerCentralManager.DeviceStateChange += CentralManager_DeviceStateChange;
            WebServerCentralManager.DiscoveredDevice += CentralManager_DiscoveredDevice;
            WebServerCentralManager.Start();
            progressBar.Maximum = 100;
            progressBar.Value = 100;
        }

        private void HttpPeripheral_ReceivedData(object source, ReceivedDataEventArgs args)
        {
            var str = DataConversion.ByteArrayToAsciiString(args.ReceivedData);
            Debug.WriteLine(str);
        }

        private void HttpPeripheral_SentData(object source, SentDataEventArgs args)
        {
            var str = DataConversion.ByteArrayToAsciiString(args.SentData);
            Debug.WriteLine(str);
        }

        private void HttpPeripheral_DeviceStateChange(object source, DeviceStateChangedEventArgs args)
        {
            Debug.WriteLine(args.PeripheralInfo.Name);
        }

        private async void CentralManager_DeviceStateChange(object source, DeviceStateChangeEventArgs args)
        {
            Debug.WriteLine(args.DeviceState.State);
        }

        private async void CentralManager_DiscoveredDevice(object source, DiscoveredDeviceEventArgs args)
        {
            if(args.DiscoveredPeripheral != null)
            {
                var httpPeripheral = args.DiscoveredPeripheral as WebServerPeripheral;
                var discoveredDeviceName = httpPeripheral.PeripheralInfo.Name;

                //IPComboBox.Items.Add(discoveredDeviceName);
                //IPComboBox.SelectedIndex++;
                await WebServerCentralManager.Connect(httpPeripheral);
                //httpPeripheral.Start();
            }
        }

        public async void Search(string HostIDOne, string HostIDTwo, string NetworkIDOne, string NetworkIDTwo, int timeout, int start, int end)
        { 
            var approximateNetwork = DataConversion.GetHttpStringFromStrings(HostIDOne, HostIDTwo, NetworkIDOne, NetworkIDTwo);
            try
            {
                Uri approximateNetworkUri = new Uri(approximateNetwork);
                if (approximateNetwork != null)
                {
                    WebServerCentralManager.SetApproximateNetwork(approximateNetworkUri);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var deviceState = WebServerCentralManager.GetDeviceState();
            if (deviceState.State == DeviceState.States.On)
            {
                WebServerCentralManager.Search(start, end, timeout);
            }
            else
            {
                DialogBoxYesOrNo dialogButton = new DialogBoxYesOrNo();
                var result = await dialogButton.ShowDialogBox("It doesn't look like WiFi is on. Go to settings?");
                if (result)
                {
                    bool settingsResult = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:network-wifi"));
                }
            }
        }
    }
}
