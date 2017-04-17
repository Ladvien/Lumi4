using Lumi4.LumiCommunication.CentralManager;
using Lumi4.LumiCommunication.DataHandling;
using Lumi4.LumiCommunication.PeripheralManager;
using Lumi4.UI;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.ApplicationModel;

namespace Lumi4.Lumi4App.ViewModels
{
    public class MainPageViewModel: BindableBase
    {
        #region properties
        private WebServerCentralManager WebServerCentralManager;

        public enum CentralDeviceType
        {
            Http,
            BluetoothLE,
            Serial
        }
        public CentralDeviceType CentralDeviceTypeSelected { get; set; }

        private ObservableCollection<Peripheral> _DiscoveredPeripherals = new ObservableCollection<Peripheral>();
        public ObservableCollection<Peripheral> DiscoveredPeripherals
        {
            get { return _DiscoveredPeripherals; }
            set { SetProperty(ref _DiscoveredPeripherals, value); }
        }

        private int _DiscoveredPeripheralIndex = -1;
        public int DiscoveredPeripheralIndex
        {
            get { return _DiscoveredPeripheralIndex; }
            set { SetProperty(ref _DiscoveredPeripheralIndex, value);}
        }

        private int _DeviceTypePivotIndex = 0;
        public int DeviceTypePivotIndex
        {
            get { return _DeviceTypePivotIndex; }
            set { SetProperty(ref _DeviceTypePivotIndex, value);
                CentralDeviceTypeSelected = (CentralDeviceType)value;
            }
        }

        private bool _WebServerSelected = true;
        public bool WebServerSelected
        {
            get { return _WebServerSelected; }
            set{ SetProperty(ref _WebServerSelected, value); }
        }

        private string _HostIDOne;
        public string HostIDOne
        {
            get { return _HostIDOne; }
            set { SetProperty(ref _HostIDOne, value); }
        }

        private string _HostIDTwo;
        public string HostIDTwo
        {
            get { return _HostIDTwo; }
            set { SetProperty(ref _HostIDTwo, value); }
        }

        private string _NetworkIDOne;
        public string NetworkIDOne
        {
            get { return _NetworkIDOne; }
            set { SetProperty(ref _NetworkIDOne, value); }
        }

        private string _NetworkIDTwo;
        public string NetworkIDTwo
        {
            get { return _NetworkIDTwo; }
            set { SetProperty(ref _NetworkIDTwo, value); }
        }

        private int _ProgressBarValue = 0;
        public int ProgressBarValue
        {
            get { return _ProgressBarValue; }
            set { SetProperty(ref _ProgressBarValue, value);
                if (value == ProgressBarMaximum)
                {
                    ProgressBarValue = 0;
                    ProgressBarMaximum = 0;
                }; 
            }
        }

        private int _ProgressBarMaximum = 0;
        public int ProgressBarMaximum
        {
            get { return _ProgressBarMaximum; }
            set { SetProperty(ref _ProgressBarMaximum, value); }
        }
        #endregion

        #region commands
        public DelegateCommand SearchCommand { get; set; }
        private bool SearchCanExecute()
        {
            return (CentralDeviceTypeSelected == CentralDeviceType.Http &&
                    CheckForValidApproximateNetWorkEntered() == true
                ) ? true : false;
        }
        private void SearchExecute()
        {
            int start = 90;
            int end = 125;
            int timeout = 300;
            ProgressBarMaximum = end - start;
            Search(HostIDOne, HostIDTwo, NetworkIDOne, NetworkIDTwo, timeout, start, end);
        }
        #endregion

        public MainPageViewModel()
        {
            WebServerCentralManager = new WebServerCentralManager();
            WebServerCentralManager.DiscoveredDevice += WebServerCentralManager_DiscoveredDevice;
            WebServerCentralManager.DeviceStateChange += CentralManager_DeviceStateChange;
            WebServerCentralManager.Start();

            Windows.ApplicationModel.Core.CoreApplication.Suspending += CoreApplication_Suspending;

            SearchCommand = new DelegateCommand(SearchExecute, SearchCanExecute).
                ObservesProperty(() => this.DeviceTypePivotIndex).
                ObservesProperty(() => this.HostIDOne).
                ObservesProperty(() => this.HostIDTwo).
                ObservesProperty(() => this.NetworkIDOne);

            LoadSettings();
            InitializeSettings();
        }

        private void WebServerCentralManager_DiscoveredDevice(object source, DiscoveredDeviceEventArgs args)
        {
            ProgressBarValue++;
            if (args.DiscoveredPeripheral != null)
            {
                var httpPeripheral = args.DiscoveredPeripheral as WebServerPeripheral;
                var discoveredDeviceName = httpPeripheral.PeripheralInfo.Name;
                try
                {
                    DiscoveredPeripherals.Add(httpPeripheral);
                    DiscoveredPeripheralIndex++;
                    Debug.WriteLine("HERE");
                } catch (Exception ex)
                {
                    Debug.WriteLine("Exception adding DiscoveredPeripheral: " + ex.Message);
                }

                //await WebServerCentralManager.Connect(httpPeripheral);
                //httpPeripheral.Start();
            }
        }

        private void CoreApplication_Suspending(object sender, SuspendingEventArgs e)
        {
            SaveSettings();
        }

        #region methods

        private bool CheckForValidApproximateNetWorkEntered()
        {
            return !String.IsNullOrWhiteSpace(HostIDOne) && !String.IsNullOrWhiteSpace(HostIDTwo) && !String.IsNullOrWhiteSpace(NetworkIDOne) ? true : false;
        }

        public void InitializeSettings()
        {

        }

        public void LoadSettings()
        {
            HostIDOne = Lumi4SettingsLibrary.LoadSetting(nameof(HostIDOne));
            HostIDTwo = Lumi4SettingsLibrary.LoadSetting(nameof(HostIDTwo));
            NetworkIDOne = Lumi4SettingsLibrary.LoadSetting(nameof(NetworkIDOne));
            NetworkIDTwo = Lumi4SettingsLibrary.LoadSetting(nameof(NetworkIDTwo));
        }

        public void SaveSettings()
        { 
            try
            {
                Lumi4SettingsLibrary.SaveSetting(nameof(HostIDOne), HostIDOne);
                Lumi4SettingsLibrary.SaveSetting(nameof(HostIDTwo), HostIDTwo);
                Lumi4SettingsLibrary.SaveSetting(nameof(NetworkIDOne), NetworkIDOne);
                Lumi4SettingsLibrary.SaveSetting(nameof(NetworkIDTwo), NetworkIDTwo);
            }
            catch (Exception ex)
            {

            }
        }


        #endregion



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
