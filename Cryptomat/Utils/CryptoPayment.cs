using System.Net;
using System.Text;

namespace Cryptomat.Utils
{
    class CryptoPayment
    {
        private static readonly WebClient WebClient = new WebClient { Encoding = Encoding.UTF8 };

        private string _mainPassword;
        private string _secondPassword;

        public void Pay(string to, int amount)
        {
            //var url = 
            //var response = WebClient.DownloadString()
        }
    }
}
