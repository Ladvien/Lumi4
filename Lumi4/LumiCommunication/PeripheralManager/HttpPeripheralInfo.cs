using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumi4.LumiCommunication.PeripheralManager
{
    public class HttpPeripheralInfo: PeripheralInfo
    {
        #region fields

        #endregion fields

        #region properties
        public Uri IP { get; protected set; }
        #endregion
        public HttpPeripheralInfo(Uri ip, string name)
        {
            Name = name;
            IP = ip;
        }
    }
}
