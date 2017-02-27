using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Lumi4.LumiCommunication.PeripheralManager;
using Lumi4;
using System.Diagnostics;
using Lumi4.LumiCommunication.DataHandling;
using Lumi4.LumiCommunication.CentralManager;
using Windows.UI.Popups;
using Lumi4.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lumi4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const string serverUrl = "http://192.168.1.103/";
        WifiCentralManager centralManager = new WifiCentralManager(serverUrl);
        HttpPeripheral Peripheral;

        public MainPage()
        {
            this.InitializeComponent();

            // UI Hacking.
            // TODO: Reconsider below.
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            byte[] testPacket = { 0x48, 0x45, 0x59, 0x20, 0x59, 0x4F, 0x55 };

            const string serverUrl = "http://192.168.1.103/";
            centralManager = new WifiCentralManager(serverUrl);

            centralManager.DeviceStateChange += CentralManager_DeviceStateChange;
            centralManager.DiscoveredDevice += CentralManager_DiscoveredDevice;
            centralManager.Start();
            
        }

        private async void CentralManager_DeviceStateChange(object source, DeviceStateChangeEventArgs args)
        {
            Debug.WriteLine(args.DeviceState.State);           
        }

        private void CentralManager_DiscoveredDevice(object source, DiscoveredDeviceEventArgs args)
        {
            var httpPeripheral = args.DiscoveredPeripheral as HttpPeripheral;
            var discoveredDeviceName = httpPeripheral.PeripheralInfo.Name;
            IPComboBox.Items.Add(discoveredDeviceName);
            IPComboBox.SelectedIndex++;

            httpPeripheral.Start();

            args.DiscoveredPeripheral.AddStringToSendBuffer("Hey you!");
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

        private async void Get_Button_Click(object sender, RoutedEventArgs e)
        {
             
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if(Peripheral != null)
            {
                Peripheral.AddDataToSendBuffer(DataConversion.StringToListByteArray(SendTextBox.Text).ToArray());
                SendTextBox.Text = "";
            } else
            {
                // TODO: Add "Not Connected" to system message.
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            var deviceState = centralManager.GetDeviceState();
            if (deviceState.State == DeviceState.States.On)
            {
                centralManager.Search(99, 112, 300, ProgressBar);
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
