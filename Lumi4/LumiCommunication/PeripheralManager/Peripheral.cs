using Lumi4.LumiCommunication.DataHandling;
using Lumi4.LumiCommunication.PeripheralManager.PeripheralEventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lumi4.LumiCommunication.PeripheralManager
{
    public abstract class Peripheral
    {
        public event DeviceStateChangeEventHandler DeviceStateChange;
        public event ReceivedDataEventHandler ReceivedData;
        public event SentDataEventHandler SentData;

        #region fields
        abstract protected PeripheralBehavior PeripheralBehavior { get; set; }

        protected List<byte> _ReceivedBuffer = new List<byte>();
        protected List<byte> _SentBuffer = new List<byte>();
        protected List<byte> _DataToSendBuffer = new List<byte>();

        #endregion fields

        #region constructor

        #endregion constructor

        #region properties


        protected CancellationTokenSource PollingForDataCancelToken = new CancellationTokenSource();
        protected CancellationTokenSource PollingSendDataCancelToken = new CancellationTokenSource();


        #endregion properties

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
                var listByteArray = DataConversion.StringToListByteArray(str);
                AddDataToSendBuffer(listByteArray.ToArray());
                return true;
            } catch (Exception ex)
            {
                Debug.WriteLine("Exception in AddStringToSendBuffer: " + ex.Message);
                return false;
            }
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
                PeripheralBehavior = peripheralBehavior;
                return true;
            } catch (Exception ex)
            {
                Debug.WriteLine("Exception in SetBehavior(): " + ex.Message);
                return false;
            }
            
        }

        abstract public bool Start(bool pollingService = false);

        abstract public void End();

    }







}
