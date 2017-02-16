using Lumi4.DeviceState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Radios;

namespace Lumi4.LumiCommunication.CentralManager
{
    class WifiCentralManager: Central
    {
        public override async void Start()
        {
            DeviceState.DeviceState deviceState = new DeviceState.DeviceState();
            var wifiOn = await IsWifiOn();
            if (wifiOn) { deviceState.State = States.On; } else { deviceState.State = States.Off; }
            OnDeviceStateChange(deviceState);
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
