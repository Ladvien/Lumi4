using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumi4.LumiCommunication.DataHandling
{
    class DataConversion
    {
        public DataConversion()
        {

        }

        public List<byte> StringToListByteArray(string stringToConvert)
        {
            List<byte> byteArray = new List<byte>();

            foreach(var character in stringToConvert)
            {
                try
                {
                    byteArray.Add(Convert.ToByte(character));
                } catch (Exception ex)
                {
                    Debug.WriteLine("Exception in StringToListByteArray: " + ex.Message);
                }
            }

            return byteArray;
        }

        public string ByteArrayToAsciiString(byte[] data)
        {
            try
            {
                return Encoding.UTF8.GetString(data);
            } catch (Exception ex)
            {
                Debug.WriteLine("Exception in ByteArrayToAsciiString: " + ex.Message);
            }
            return "";
        }
    }
}
