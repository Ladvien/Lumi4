using Lumi4.DeviceState;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Radios;
using Lumi4.LumiCommunication.DataHandling;
using System.Net.Http;
using Windows.UI.Xaml.Controls;

namespace Lumi4.LumiCommunication.CentralManager
{
    class WifiCentralManager: Central
    {
        #region properties
        private Uri IP { get; set; }
        private int PollingDelay { get; set; }
        private bool PollingActive { get; set; } = false;
        public string WebServerUrl { get; private set; }
        HttpClient httpClient = new HttpClient();
        CancellationTokenSource PollingForDataCancelToken = new CancellationTokenSource();
        #endregion

        public WifiCentralManager(string ip)
        {
            try
            {
                IP = new Uri(ip);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to construct WifiCentralManager: " + ex.Message);
            }
        }

        public override async void Start()
        {
            DeviceState.DeviceState deviceState = new DeviceState.DeviceState();
            var wifiOn = await IsWifiOn();
            if (wifiOn) { deviceState.State = States.On; } else { deviceState.State = States.Off; }
            OnDeviceStateChange(deviceState);
        }


        public async Task<List<Uri>> Search(int startIndex, int endIndex, int timeout = 300, ProgressBar progressBar = null)
        {
            DataConversion dataConverter = new DataConversion();
            var threePartIP = DataConversion.seperateStringByCharacterIndex(IP.ToString(), 3, '.');
            var httpClient = new System.Net.Http.HttpClient();
            httpClient.Timeout = new TimeSpan(0, 0, 0, 0, timeout);
            var webService = WebServerUrl + "name";
            List<Uri> discoveredIPs = new List<Uri>();

            if (progressBar != null)
            {
                progressBar.Maximum = endIndex - startIndex;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                try
                {
                    string ip = threePartIP + i.ToString() + "/";
                    var resourceUri = new Uri(ip);
                    var response = await httpClient.PostAsync(resourceUri, null);
                    if (response.IsSuccessStatusCode == true)
                    {
                        discoveredIPs.Add(resourceUri);
                    }
                    response.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in WifiCentralManager.Search: " + ex.Message);
                }
                if(progressBar != null) { progressBar.Value += 1; }
                
            }
            if(progressBar != null)
            {
                progressBar.Value = 0;
                progressBar.IsEnabled = false;
            }
            return discoveredIPs;
        }

        public async Task<bool> IsWifiOn()
        {
            await Radio.RequestAccessAsync();

            var radios = await Radio.GetRadiosAsync();
            foreach (var radio in radios)
            {
                if (radio.Kind == RadioKind.WiFi)
                {
                    return radio.State == RadioState.On;
                }
            }
            return false;
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

        public async Task<string> GetData()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));

            var webService = WebServerUrl + "buffer";
            var resourceUri = new Uri(webService);
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(resourceUri, null);
                var message = await response.Content.ReadAsStringAsync();
                if (message != "") { Debug.WriteLine(message); }
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
    }

}

