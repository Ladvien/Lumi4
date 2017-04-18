using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumi4.DeviceState;
using Lumi4.LumiCommunication.PeripheralManager.PeripheralEventArgs;

namespace Lumi4.LumiCommunication.PeripheralManager
{
    public delegate void ReceivedDataEventHandler(object source, ReceivedDataEventArgs args);
    public delegate void SentDataEventHandler(object source, SentDataEventArgs args);
    public delegate void DeviceStateChangeEventHandler(object source, DeviceStateChangedEventArgs args);

    interface IPeripheral
    {
        #region delegates and events
        // Delegate info:
        // https://www.youtube.com/watch?v=jQgwEsJISy0
        event ReceivedDataEventHandler ReceivedData;
        void OnReceivedData(byte[] data);
        event SentDataEventHandler SentData;
        void OnSentData(byte [] data);
        event DeviceStateChangeEventHandler DeviceStateChange;
        void OnDeviceStateChange(PeripheralInfo peripheralInfo);

        #endregion delegates and events



        #region properties
        PeripheralInfo PeripheralInfo { get; }
        PeripheralBehavior PeripheralBehavior { get; }
        List<byte> ReceivedBufferUpdated { get; }
        List<byte> SentBufferUpdated { get; }

        #endregion properties

        #region methods

        void Start();
        void End();
        bool AddDataToSendBuffer(byte[] data);
        bool AddStringToSendBuffer(string str);
        bool SetBehavior(PeripheralBehavior peripheralBehavior);
        PeripheralInfo GetDeviceInfo();

        #endregion methods
    }
}
