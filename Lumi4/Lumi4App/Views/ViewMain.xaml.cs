using Lumi4.LumiCommunication.CentralManager;
using Lumi4.LumiCommunication.DataHandling;
using Lumi4.LumiCommunication.PeripheralManager;
using Lumi4.UI;
using System;
using System.Diagnostics;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lumi4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewMain : Page
    {
        const string serverUrl = "http://192.168.1.103/";
        static Uri serverUri = new Uri(serverUrl);
        WebServerCentralManager centralManager = new WebServerCentralManager(serverUri);
        WebServerPeripheral Peripheral;

        public ViewMain()
        {
            this.InitializeComponent();
            InitializeSettings();
            DataContext = new Lumi4App.ViewModels.ViewMainViewModel();
            Windows.ApplicationModel.Core.CoreApplication.Suspending += CoreApplication_Suspending;

        }

        
        private void CoreApplication_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            SaveSettings();
        }
     
        public void SaveSettings()
        {
            var mainViewViewModel = DataContext as Lumi4App.ViewModels.ViewMainViewModel;
            mainViewViewModel.SaveSettings();
        }

        public void InitializeSettings()
        {

            // UI Hacking.
            // TODO: Reconsider below.
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            byte[] testPacket = { 0x48, 0x45, 0x59, 0x20, 0x59, 0x4F, 0x55 };

            string serverUrl = "http://192.168.1.103/";
            Uri serverUri = new Uri(serverUrl);
            centralManager = new WebServerCentralManager(serverUri);

            centralManager.DeviceStateChange += CentralManager_DeviceStateChange;
            centralManager.DiscoveredDevice += CentralManager_DiscoveredDevice;
            centralManager.Start();

            ProgressBar.Maximum = 100;
            ProgressBar.Value = 100;


        }

        private async void CentralManager_DeviceStateChange(object source, DeviceStateChangeEventArgs args)
        {
            Debug.WriteLine(args.DeviceState.State);           
        }

        private async void CentralManager_DiscoveredDevice(object source, DiscoveredDeviceEventArgs args)
        {
            var httpPeripheral = args.DiscoveredPeripheral as WebServerPeripheral;
            var discoveredDeviceName = httpPeripheral.PeripheralInfo.Name;

            IPComboBox.Items.Add(discoveredDeviceName);
            IPComboBox.SelectedIndex++;
            await centralManager.Connect(httpPeripheral);
            //httpPeripheral.Start();
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


            var approximateNetwork = DataConversion.GetHttpStringFromStrings(NetworkIDOne.Text,
                                                                             NetworkIDTwo.Text,
                                                                             HostIDOne.Text,
                                                                             HostIDTwo.Text);
            try
            {
                Uri approximateNetworkUri = new Uri(approximateNetwork);
                if (approximateNetwork != null)
                {
                    centralManager.SetApproximateNetwork(approximateNetworkUri);
                }
            } catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
            var deviceState = centralManager.GetDeviceState();
            if (deviceState.State == DeviceState.States.On)
            {
                centralManager.Search(90, 125, 300, ProgressBar);
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

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IPComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = IPComboBox.SelectedItem.ToString();
            if (name != "" && name != null)
            {
                var httpPeripheral = centralManager.GetDiscoveredPeripheralByName(name);
                SelectedIp.Text = httpPeripheral.PeripheralInfo.IP.ToString();
            }
        }


    }
}
