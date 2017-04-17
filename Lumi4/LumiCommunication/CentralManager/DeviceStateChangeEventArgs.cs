using System;

namespace Lumi4.LumiCommunication.CentralManager
{
    public class DeviceStateChangeEventArgs: EventArgs
    {
        public DeviceState.DeviceState DeviceState { get; set; }
    }

    

}
