using Lumi4.DeviceState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Radios;
using Lumi4.LumiCommunication.PeripheralManager;

namespace Lumi4.LumiCommunication.CentralManager
{
    public delegate void CentralStateChangeEventHandler(object source, DeviceStateChangeEventArgs args);
    public delegate void DiscoveredDeviceEventHandler(object source, DiscoveredDeviceEventArgs args);

    abstract public class Central
    {

       public Central()
        {
            DeviceState = new DeviceState.DeviceState();
            CentralBehavior = new CentralBehavior();
        }

        #region delegates and events
        public event CentralStateChangeEventHandler DeviceStateChange;
        public event DiscoveredDeviceEventHandler DiscoveredDevice;
        #endregion delegates and events

        #region fields

        #endregion fields

        #region properties
        protected DeviceState.DeviceState DeviceState { get; set; }
        protected CentralInfo CentralInfo { get; set; }
        protected CentralBehavior CentralBehavior { get; set; }

        #endregion properties

        #region methods

        public abstract void Start();
        public void OnDeviceStateChange(DeviceState.DeviceState deviceState)
        {
            DeviceStateChangeEventArgs deviceStateChangeEventArgs = new DeviceStateChangeEventArgs();
            deviceStateChangeEventArgs.DeviceState = deviceState;
            DeviceStateChange?.Invoke(this, deviceStateChangeEventArgs);
        }

        public void OnDiscoveringDevice(Peripheral peripheral)
        {
            DiscoveredDeviceEventArgs args = new DiscoveredDeviceEventArgs();
            args.DiscoveredPeripheral = peripheral;
            DiscoveredDevice?.Invoke(this, args);
        }

        public DeviceState.DeviceState GetDeviceState()
        {
            return this.DeviceState;
        }

        #endregion methods
    }

    public class DeviceStateChangeEventArgs: EventArgs
    {
        public DeviceState.DeviceState DeviceState { get; set; }
    }

    public class DiscoveredDeviceEventArgs: EventArgs
    {
        public Peripheral DiscoveredPeripheral { get; set; }
    }
}
