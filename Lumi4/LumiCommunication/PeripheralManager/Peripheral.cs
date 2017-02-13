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

        }
        #endregion constructor

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

            set
            {
                _PeripheralInfo = value;
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
            try
            {

            } catch (Exception ex)
            {

            }
        }

        public PeripheralInfo GetDeviceInfo()
        {
            throw new NotImplementedException();
        }

        public void OnDeviceStateChange()
        {
            throw new NotImplementedException();
        }

        public void OnReceivedData()
        {
            throw new NotImplementedException();
        }

        public void OnSentData()
        {
            throw new NotImplementedException();
        }

        public bool SetBehavior(PeripheralBehavior peripheralBehavior)
        {
            throw new NotImplementedException();
        }
    }
}
