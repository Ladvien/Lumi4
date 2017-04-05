using System;
using Lumi4.LumiCommunication.CentralManager;
using Lumi4.LumiCommunication.PeripheralManager;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Lumi4.IntegrationTests
{
    public class Lumi4IntegrationTestSettings
    {
        public static Uri LocalIP = new Uri("http://192.168.1.100/");
        public static int SearchWifiCallbackDelay = 30000;
    }

    [TestClass]
    public class WifiCentralManagerTests
    {
        [TestClass]
        public class Search
        {
            [TestMethod]
            public async Task Search_FindsWebServer_ValidIp()
            {
                var localNetwork = Lumi4IntegrationTestSettings.LocalIP;
                WifiCentralManager wifiCentralManager = new WifiCentralManager(localNetwork);
                bool foundDevice = false;
                wifiCentralManager.DiscoveredDevice += delegate (object obj, DiscoveredDeviceEventArgs args)
                {
                    if (args.DiscoveredPeripheral != null) { foundDevice = true; }
                };
                wifiCentralManager.Search(90, 120);
                await Task.Delay(Lumi4IntegrationTestSettings.SearchWifiCallbackDelay);
                Assert.IsTrue(foundDevice);
            }
        }
    }
}

