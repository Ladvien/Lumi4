﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumi4.LumiCommunication.DeviceState;

namespace Lumi4.LumiCommunication.PeripheralManager
{
    public class WebServerPeripheralInfo: PeripheralInfo
    {
        #region fields

        #endregion fields

        #region properties
        public Uri IP { get; protected set; }
        #endregion
        public WebServerPeripheralInfo(Uri ip, string name)
        {
            Name = name;
            IP = ip;
            DeviceState = new DeviceState.DeviceState();
        }
    }
}
