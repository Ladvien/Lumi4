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
        private WebServerCentralManager centralManager;
        private ProgressBar progressBar = new ProgressBar();

        public Lumi4Model()
        {
            InitializeSettings();
        }

        public void InitializeSettings()
        {

            byte[] testPacket = { 0x48, 0x45, 0x59, 0x20, 0x59, 0x4F, 0x55 };

            string serverUrl = "http://192.168.1.103/";
            Uri serverUri = new Uri(serverUrl);
            centralManager = new WebServerCentralManager(serverUri);

            centralManager.DeviceStateChange += CentralManager_DeviceStateChange;
            centralManager.DiscoveredDevice += CentralManager_DiscoveredDevice;
            centralManager.Start();

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
            var httpPeripheral = args.DiscoveredPeripheral as WebServerPeripheral;
            var discoveredDeviceName = httpPeripheral.PeripheralInfo.Name;

            //IPComboBox.Items.Add(discoveredDeviceName);
            //IPComboBox.SelectedIndex++;
            await centralManager.Connect(httpPeripheral);
            //httpPeripheral.Start();
        }

        private async void Search(string HostIDOne, string HostIDTwo, string NetworkIDOne, string NetworkIDTwo)
        { 
            var approximateNetwork = DataConversion.GetHttpStringFromStrings(NetworkIDOne,
                                                                             NetworkIDTwo,
                                                                             HostIDOne,
                                                                            HostIDTwo);
            try
            {
                Uri approximateNetworkUri = new Uri(approximateNetwork);
                if (approximateNetwork != null)
                {
                    centralManager.SetApproximateNetwork(approximateNetworkUri);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var deviceState = centralManager.GetDeviceState();
            if (deviceState.State == DeviceState.States.On)
            {
                centralManager.Search(90, 125, 300, progressBar);
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
