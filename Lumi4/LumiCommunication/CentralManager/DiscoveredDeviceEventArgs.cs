using System;
using Lumi4.LumiCommunication.PeripheralManager;

namespace Lumi4.LumiCommunication.CentralManager
{
    public class DiscoveredDeviceEventArgs: EventArgs
    {
        public Peripheral DiscoveredPeripheral { get; set; }
        public int SearchIndex { get; set; }
    }

      

}
