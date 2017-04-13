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

        public static List<String> LoadNetworkSettings()
        {
            List<String> networkParts = new List<string>();
            string NetworkIDOne, NetworkIDTwo, HostIDOne, HostIDTwo;
            try
            {
                Windows.Storage.ApplicationDataContainer Lumi4AppSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                Windows.Storage.StorageFolder Lumi4AppFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

                NetworkIDOne = Lumi4AppSettings.Values["NetworkIDOne"] as string;
                NetworkIDTwo = Lumi4AppSettings.Values["NetworkIDTwo"] as string;
                HostIDOne = Lumi4AppSettings.Values["HostIDOne"] as string;
                HostIDTwo = Lumi4AppSettings.Values["HostIDTwo"] as string;


                networkParts.Add(NetworkIDOne);
                networkParts.Add(NetworkIDTwo);
                networkParts.Add(HostIDOne);
                networkParts.Add(HostIDTwo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadNetworkSettings Ex.: " + ex.Message);
            }
            return networkParts;
        }

        public static void SaveNetworkSettings(string NetworkIDOne, string NetworkIDTwo, string HostIDOne, string HostIDTwo)
        {
            Windows.Storage.ApplicationDataContainer Lumi4AppSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Windows.Storage.StorageFolder Lumi4AppFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            Lumi4AppSettings.Values["NetworkIDOne"] = NetworkIDOne;
            Lumi4AppSettings.Values["NetworkIDTwo"] = NetworkIDTwo;
            Lumi4AppSettings.Values["HostIDOne"] = HostIDOne;
            Lumi4AppSettings.Values["HostIDTwo"] = HostIDTwo;
        }
    }
}
