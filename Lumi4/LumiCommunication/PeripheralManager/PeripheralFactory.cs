using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumi4.LumiCommunication.PeripheralManager;

namespace Lumi4.LumiCommunication.PeripheralManager
{
    static class PeripheralFactory
    {
        static public Peripheral CreateNewPeripheral(string peripheralType)
        {
            switch (peripheralType)
            {
                case "bluetooth":
                    return new BluetoothPeripheral();
                case "http":
                    return new HttpPeripheral();
                default:
                    return null;
            }
        }
    }
}
