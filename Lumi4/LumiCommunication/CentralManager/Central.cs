using Lumi4.DeviceState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Radios;


namespace Lumi4.LumiCommunication.CentralManager
{
    public delegate void CentralStateChangeEventHandler(object source, DeviceStateChangeEventArgs args);

    abstract public class Central
    {

        #region delegates and events
        public event CentralStateChangeEventHandler DeviceStateChange;

        #endregion delegates and events

        #region fields

        #endregion fields

        #region properties
        private DeviceState.DeviceState DeviceState { get; set; }
        private CentralInfo CentralInfo { get; set; }
        private CentralBehavior CentralBehavior { get; set; }

        #endregion properties

        #region methods

        public abstract void Start();
        public void OnDeviceStateChange(DeviceState.DeviceState deviceState)
        {
            DeviceStateChangeEventArgs deviceStateChangeEventArgs = new DeviceStateChangeEventArgs();
            deviceStateChangeEventArgs.DeviceState = deviceState;
            DeviceStateChange?.Invoke(this, deviceStateChangeEventArgs);
        }
        #endregion methods
    }

    public class DeviceStateChangeEventArgs: EventArgs
    {
        public DeviceState.DeviceState DeviceState { get; set; }
    }
}
