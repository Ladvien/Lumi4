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
            if(args.DeviceState.State == DeviceState.States.On)
            {
                centralManager.Search(99, 120, 300, ProgressBar);
            } else
            {

                var dialog = new MessageDialog("It doesn't look like WiFi is on. Go to settings?");

                var yesCommand = new UICommand("Yes") { Id = 0 };
                var noCommand = new UICommand("No") { Id = 1 };
                dialog.Commands.Add(yesCommand);
                dialog.Commands.Add(noCommand);

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                var result = await dialog.ShowAsync();

                if(result == yesCommand)
                {
                    bool settingsResult = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:network-wifi"));
                }
                
            }
            
        }

        private void CentralManager_DiscoveredDevice(object source, DiscoveredDeviceEventArgs args)
        {
            Debug.WriteLine(args.Peripherals[0].PeripheralInfo.Name);
        }

        private void HttpPeripheral_ReceivedData(object source, ReceivedDataEventArgs args)
        { 
            DataConversion converter = new DataConversion();
            var str = converter.ByteArrayToAsciiString(args.ReceivedData);
            Debug.WriteLine(str);
        }

        private void HttpPeripheral_SentData(object source, SentDataEventArgs args)
        {
            DataConversion converter = new DataConversion();
            var str = converter.ByteArrayToAsciiString(args.SentData);
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

        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
