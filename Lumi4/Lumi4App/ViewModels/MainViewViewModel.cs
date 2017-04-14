using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumi4.Lumi4App;
using System.Diagnostics;

namespace Lumi4.Lumi4App.ViewModels
{
    public class MainViewViewModel: MainViewModelBase
    {

        #region properties
        private string _HostIDOne;
        public string HostIDOne
        {
            get { return _HostIDOne; }
            set {
                if (_HostIDOne != value)
                {
                    _HostIDOne = value;
                }
            }
        }

        private string _HostIDTwo;

        public string HostIDTwo
        {
            get { return _HostIDTwo; }
            set {
                if (_HostIDTwo != value)
                {
                    _HostIDTwo = value;
                }
            }
        }

        private string _NetworkIDOne;

        public string NetworkIDOne
        {
            get { return _NetworkIDOne; }
            set {
                if (_NetworkIDOne != value)
                {
                    _NetworkIDOne = value;
                }
            }
        }

        private string _NetworkIDTwo;

        public string NetworkIDTwo
        {
            get { return _NetworkIDTwo; }
            set {
                if (_NetworkIDTwo != value)
                {
                    _NetworkIDTwo = value;
                }
            }
        }
        #endregion

        public MainViewViewModel()
        {
            LoadSettings();
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
    }
}
