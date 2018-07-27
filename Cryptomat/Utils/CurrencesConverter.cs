using System;
using System.Net;
using System.Text;
using System.Timers;
using Newtonsoft.Json;

namespace Cryptomat.Utils
{
    class CurrencesConverter
    {
        //private static readonly WebClient WebClient = new WebClient {Encoding = Encoding.UTF8};
        private const string BynToUsdUrl = "http://www.nbrb.by/API/ExRates/Rates/145";
        private const string UsdToBtcUrl = "https://api.cryptonator.com/api/full/btc-usd";

        public static double BynUsdCourse { get; private set; }
        public static double UsdBtcCourse { get; private set; }

        public static Timer _updateTimer;

        public static void Start()
        {
            _updateTimer = new Timer
            {
                Interval = 2000
            };
            _updateTimer.Elapsed += UpdateCourses;
            _updateTimer.Start();
        }

        private static void UpdateCourses(object sender, ElapsedEventArgs e)
        {
            BynUsdCourse = BynToUsd();
            UsdBtcCourse = UsdToBtc();
        }

        private static double BynToUsd()
        {
            //try
            //{
            var WebClient = new WebClient { Encoding = Encoding.UTF8 };
            var jsonString = WebClient.DownloadString(BynToUsdUrl);
            dynamic response = JsonConvert.DeserializeObject(jsonString);
            return response.Cur_OfficialRate;
            //}

            //catch (Exception)
            //{
            //    return 0.0;
            //}
        }

        private static double UsdToBtc()
        {
            try
            {
                var WebClient = new WebClient { Encoding = Encoding.UTF8 };
                var jsonString = WebClient.DownloadString(UsdToBtcUrl);
                dynamic response = JsonConvert.DeserializeObject(jsonString);
                return response.ticker.markets[0].price;
            }

            catch (Exception)
            {
                return 0.0;
            }
        }

        public static double BynToBtc(double amount)
        {
            return amount / BynUsdCourse / UsdBtcCourse;
        }
    }
}
