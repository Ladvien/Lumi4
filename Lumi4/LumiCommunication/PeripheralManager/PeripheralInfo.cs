﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumi4.LumiCommunication.PeripheralManager
{
    abstract public class PeripheralInfo
    {
        public string Name { get; set; }
        public DeviceState.DeviceState DeviceState { get; set; }
    }
}
