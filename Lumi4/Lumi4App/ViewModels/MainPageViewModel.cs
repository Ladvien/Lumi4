using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumi4.Lumi4App;
using System.Diagnostics;
using Windows.UI.Xaml;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Lumi4.Lumi4App.Models;
using Windows.ApplicationModel;

namespace Lumi4.Lumi4App.ViewModels
{
    public class MainPageViewModel: BindableBase
    {
        #region properties
        private Lumi4Model Lumi4Model;

        public enum CentralDeviceType
        {
            Http,
            BluetoothLE,
            Serial
        }
        public CentralDeviceType CentralDeviceTypeSelected { get; set; }

        private int _DeviceTypePivotIndex;
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

        }
        #endregion

        public MainPageViewModel(Lumi4Model lumi4Model)
        {

            Windows.ApplicationModel.Core.CoreApplication.Suspending += CoreApplication_Suspending;

            Lumi4Model = lumi4Model;

            SearchCommand = new DelegateCommand(SearchExecute, SearchCanExecute).
                ObservesProperty(() => this.DeviceTypePivotIndex).
                ObservesProperty(() => this.HostIDOne).
                ObservesProperty(() => this.HostIDTwo).
                ObservesProperty(() => this.NetworkIDOne);

            LoadSettings();
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
    }
}
