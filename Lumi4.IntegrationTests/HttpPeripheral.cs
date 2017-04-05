using System;

namespace Lumi4.IntegrationTests
{
    internal class HttpPeripheral
    {
        private Uri uri;
        private string v;

        public HttpPeripheral(string v, Uri uri)
        {
            this.v = v;
            this.uri = uri;
        }
    }
}