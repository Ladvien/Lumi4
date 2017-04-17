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
    public class WebServerPeripheral: Peripheral
    {
        public const string WebServiceGetName = "getname";
        public const string WebServiceSendData = "senddata";
        public const string WebServiceSendString = "sendstring";
        public const string WebServiceGetBuffer = "getbuffer";
        public const string WebServiceConnect = "connect";
        public const string WebServiceDisconnect = "disconnect";

        #region properties
        
        public WebServerPeripheralInfo PeripheralInfo { get; }
        HttpClient httpClient = new HttpClient();

        private int PollingDelay { get; set; }
        private bool PollingActive { get; set; } = false;

        protected override PeripheralBehavior PeripheralBehavior { get; set; }

        #endregion

        public WebServerPeripheral(string name, Uri uri)
        {
            PeripheralInfo = new WebServerPeripheralInfo(uri, name);
            PeripheralBehavior = new PeripheralBehavior();
        }

        override public void Start()
        {
            PollWebServerDataAvailability();
        }

        override public void End()
        {
            throw new NotImplementedException();
        }

        public void SetPollingDelay(int delayInMilliseconds) { PollingDelay = delayInMilliseconds; }

        private void PollWebServerDataAvailability()
        {
            try
            {
                Task.Run(async () =>
                {
                    while (true)
                    {
                        if (PollingForDataCancelToken.IsCancellationRequested)
                        {
                            PollingForDataCancelToken.Token.ThrowIfCancellationRequested();
                        }
                        await GetData();
                        await Task.Delay(PollingDelay);
                    }
                }, PollingForDataCancelToken.Token);
            }
            catch (TaskCanceledException)
            {
                // TODO: Add cancelation callback here.
            }
        }

        private void PollingSendData()
        {
            try
            {
                Task.Run(async () =>
                {
                    while (true)
                    {
                        if (PollingSendDataCancelToken.IsCancellationRequested)
                        {
                            PollingSendDataCancelToken.Token.ThrowIfCancellationRequested();
                        }
                        await GetData();
                        await Task.Delay(PollingDelay);
                    }
                }, PollingSendDataCancelToken.Token);
            }
            catch (TaskCanceledException)
            {
                // TODO: Add cancelation callback here.
            }
        }

        public async Task<string> GetData()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));

            var webService = PeripheralInfo.IP + WebServiceGetBuffer;
            var resourceUri = new Uri(webService);
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(resourceUri, null);
                var message = await response.Content.ReadAsStringAsync();
                if (message != "") {
                    Debug.WriteLine(message);
                    OnReceivedData(DataHandling.DataConversion.StringToListByteArray(message).ToArray());
                }
                response.Dispose();
                cts.Dispose();
                return message;
            }
            catch (TaskCanceledException ex)
            {
                // Handle request being canceled due to timeout.
                return "";
            }
            return "";
        }


        public async Task<string> SendData(string stringToSend)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));

            var webService = PeripheralInfo.IP + WebServiceSendString + "=" + stringToSend + "/";
            var resourceUri = new Uri(webService);
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(resourceUri, null);
                var message = await response.Content.ReadAsStringAsync();
                if (message != "")
                {
                    Debug.WriteLine(message);
                    OnReceivedData(DataHandling.DataConversion.StringToListByteArray(message).ToArray());
                }
                response.Dispose();
                cts.Dispose();
                return message;
            }
            catch (TaskCanceledException ex)
            {
                // Handle request being canceled due to timeout.
                return "";
            }
            return "";
        }



        public override string ToString()
        {
            return this.PeripheralInfo.Name;
        }
    }

}
