using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumi4.LumiCommunication.PeripheralManager
{
    public class HttpPeripheral: Peripheral
    {
        #region properties
        private Uri PeripheralsUri { get; set; }
        #endregion

        public HttpPeripheral(string name, Uri uri)
        {
            PeripheralsUri = uri;
            this.PeripheralInfo.Name = name;
        }
    }
}
