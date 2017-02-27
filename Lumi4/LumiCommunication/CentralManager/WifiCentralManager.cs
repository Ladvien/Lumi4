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
using Lumi4.DeviceState;
using Lumi4.LumiCommunication.PeripheralManager;
namespace Lumi4.LumiCommunication.CentralManager
{
    class WifiCentralManager: Central
    {
        private const string WebServiceGetName = "name";

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

        public override void Start()
        {
            UpdateDeviceStateWithWifiStatus();
        }

        public async void Search(int startIndex, int endIndex, int timeout = 300, ProgressBar progressBar = null)
        {

            // 1. Convert the passed IP down to 3 places.
            // 2. Create System.Net.Http.HttpClient (NOTE: Windows HttpClient doesn't seem to have 
            //    adjustable timeout.)
            // 3. Set the POST string to "name".  This will be used as a handshake.
            // 4. Iterate over the IP range, POSTing handshake.
            // 5. If handshake successful at IP, create a peripheral and add it to list.
            // 6. Update DeviceState: Searching->On
            // 7. After iteration, return list, even if empty.
            
            DataConversion dataConverter = new DataConversion();
            var threePartIP = DataConversion.seperateStringByCharacterIndex(IP.ToString(), 3, '.');
            var httpClient = new System.Net.Http.HttpClient();
            httpClient.Timeout = new TimeSpan(0, 0, 0, 0, timeout);

            if (progressBar != null)
            {
                progressBar.Maximum = endIndex - startIndex;
            }

            DeviceState.State = States.Searching;
            OnDeviceStateChange(DeviceState);

            for (int i = startIndex; i < endIndex; i++)
            {
                try
                {
                    string ip = threePartIP + i.ToString() + "/" + WebServiceGetName;
                    var resourceUri = new Uri(ip);
                    var response = await httpClient.PostAsync(resourceUri, null);
                    if (response.IsSuccessStatusCode == true)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        if (responseString != "")
                        {
                            HttpPeripheral peripheral = new HttpPeripheral(responseString, resourceUri);
                            OnDiscoveringDevice(peripheral);
                        }
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
            UpdateDeviceStateWithWifiStatus();
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

        private async void UpdateDeviceStateWithWifiStatus()
        {
            var wifiOn = await IsWifiOn();
            if (wifiOn) { DeviceState.State = States.On; } else { DeviceState.State = States.Off; }
            OnDeviceStateChange(DeviceState);
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
    }

}

