using System;

namespace Lumi4.IntegrationTests
{
    internal class WifiCentralManager
    {
        private Uri ip;

        public WifiCentralManager(Uri ip)
        {
            this.ip = ip;
        }

        public Func<object> DiscoveredDevice { get; internal set; }
    }
}