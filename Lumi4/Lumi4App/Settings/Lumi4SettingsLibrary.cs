using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumi4
{
    static public class Lumi4SettingsLibrary
    {
        public static string LoadSetting(string name)
        {
            Windows.Storage.ApplicationDataContainer Lumi4AppSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string value = Lumi4AppSettings.Values[name] as string;
            if (value != null) { return value; }
            else { return ""; }
        }

        public static void SaveSetting(string name, string value)
        {
            Windows.Storage.ApplicationDataContainer Lumi4AppSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (name != null && value != null) { Lumi4AppSettings.Values[name] = value; }
        }
    }
}
