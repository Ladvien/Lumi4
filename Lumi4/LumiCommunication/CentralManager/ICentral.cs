using Lumi4.LumiCommunication.PeripheralManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumi4.CentralManager
{
    public delegate void DiscoveredDevice(object source, EventArgs args);
    interface ICentral
    {

        #region delegates and events
        // Delegate info:
        // https://www.youtube.com/watch?v=jQgwEsJISy0
        event ReceivedDataEventHandler ReceivedData;
        void OnReceivedData();
        #endregion delegates and events

        #region fields

        #endregion fields

        #region properties


        #endregion properties

        #region methods
        void BeginSearch();

        #endregion methods
    }
}
