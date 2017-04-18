using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumi4.LumiCommunication.PeripheralManager.PeripheralEventArgs
{
    public class SentDataEventArgs : EventArgs
    {
        internal byte[] SentData { get; set; }
    }
}
