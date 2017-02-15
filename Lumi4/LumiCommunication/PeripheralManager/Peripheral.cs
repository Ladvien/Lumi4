using Lumi4.LumiCommunication.DataHandling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumi4.LumiCommunication.PeripheralManager
{
    abstract class Peripheral: IPeripheral
    {
        public event DeviceStateChangeEventHandler DeviceStateChange;
        public event ReceivedDataEventHandler ReceivedData;
        public event SentDataEventHandler SentData;

        #region fields
        private PeripheralBehavior _PeripheralBehavior = new PeripheralBehavior();
        private PeripheralInfo _PeripheralInfo = new PeripheralInfo();

        private List<byte> _ReceivedBuffer = new List<byte>();
        private List<byte> _SentBuffer = new List<byte>();
        private List<byte> _DataToSendBuffer = new List<byte>();

        #endregion fields

        #region constructor
        internal Peripheral()
        {
            _PeripheralInfo.Name = "Bob";
        }
        #endregion constructor

        public void Test()
        {
            byte[] receivedData = new byte[] { 0x33, 0x34, 0x44 };
            byte[] sentData = new byte[] { 0x55, 0x54, 0x56 };
            OnDeviceStateChange(_PeripheralInfo);
            OnReceivedData(receivedData);
            OnSentData(sentData);
        }

        public PeripheralBehavior PeripheralBehavior
        {
            get
            {
                return _PeripheralBehavior;
            }

            set
            {
                _PeripheralBehavior = value;
            }
        }

        public PeripheralInfo PeripheralInfo
        {
            get
            {
                return _PeripheralInfo;
            }
        }

        public List<byte> ReceivedBufferUpdated
        {
            get
            {
                return _ReceivedBuffer;
            }
        }

        public List<byte> SentBufferUpdated
        {
            get
            {
                return _SentBuffer;
            }
        }

        public bool AddDataToSendBuffer(byte[] data)
        {
            try
            {
                _DataToSendBuffer.AddRange(data);
                return true;
            } catch (Exception ex)
            {
                Debug.WriteLine("AddDataToSendBuffer Exception: " + ex.Message);
                return false;
            }
        }

        public bool AddStringToSendBuffer(string str)
        {
            DataConversion dataConverter = new DataConversion();
            try
            {
                var listByteArray = dataConverter.StringToListByteArray(str);
                AddDataToSendBuffer(listByteArray.ToArray());
                return true;
            } catch (Exception ex)
            {
                Debug.WriteLine("Exception in AddStringToSendBuffer: " + ex.Message);
                return false;
            }
        }

        public PeripheralInfo GetDeviceInfo()
        {
            return _PeripheralInfo;
        }

        public void OnDeviceStateChange(PeripheralInfo peripheralInfo)
        {
            DeviceStateChangedEventArgs deviceStateChangedEventArgs = new DeviceStateChangedEventArgs();
            deviceStateChangedEventArgs.PeripheralInfo = peripheralInfo;
            DeviceStateChange?.Invoke(this, deviceStateChangedEventArgs);
        }

        public void OnReceivedData(byte[] data)
        {
            ReceivedDataEventArgs receivedArgs = new ReceivedDataEventArgs();
            receivedArgs.ReceivedData = data;
            ReceivedData?.Invoke(this, receivedArgs);
        }

        public void OnSentData(byte[] data)
        {
            SentDataEventArgs sentArgs = new SentDataEventArgs();
            sentArgs.SentData = data;
            SentData?.Invoke(this, sentArgs);
        }

        public bool SetBehavior(PeripheralBehavior peripheralBehavior)
        {
            try
            {
                _PeripheralBehavior = peripheralBehavior;
                return true;
            } catch (Exception ex)
            {
                Debug.WriteLine("Exception in SetBehavior(): " + ex.Message);
                return false;
            }
            
        }
    }

    public class DeviceStateChangedEventArgs: EventArgs
    {
        internal PeripheralInfo PeripheralInfo { get; set; }
    }

    public class ReceivedDataEventArgs : EventArgs
    {
        internal byte[] ReceivedData { get; set; }
    }

    public class SentDataEventArgs: EventArgs
    {
        internal byte[] SentData { get; set; }
    }

}
