using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumi4.DeviceState
{
    public enum States
    {
        Unknown,
        Off,
        On,
        Searching
    }

    public class DeviceState
    {
        public States State { get; set; }
    }
}
