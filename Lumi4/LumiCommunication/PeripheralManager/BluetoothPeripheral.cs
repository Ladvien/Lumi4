using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumi4.LumiCommunication.PeripheralManager
{
    class BluetoothPeripheral : Peripheral
    {
        protected override PeripheralBehavior PeripheralBehavior
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool Start(bool pollingService = false)
        {
            throw new NotImplementedException();
        }

        public override void End()
        {
            throw new NotImplementedException();
        }
    }
}
