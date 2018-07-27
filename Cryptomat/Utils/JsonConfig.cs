using System;
using System.IO;
using Newtonsoft.Json;

namespace Cryptomat.Utils
{
    public class JsonConfig
    {
        private const string FileName = "config.txt";
        public static int CameraId { get; private set; }
        public static double Commission { get; private set; }
        public static string CashPortName { get; private set; }
        public static int CashBaudRate { get; private set; }
        public static bool FullScreen { get; private set; }
        public static bool CashCodeRequired { get; private set; }
        public static bool WalletRequired { get; private set; }
        public static bool UsePrinter { get; private set; }

        public static Error.Code Init()
        {
            return LoadFromFile();
        }

        private static Error.Code LoadFromFile()
        {
            if (!File.Exists(FileName))
                return Error.Code.MissingConfigFile;
            try
            {
                var jsonString = File.ReadAllText(FileName);
                dynamic config = JsonConvert.DeserializeObject(jsonString);

                CameraId = config.CameraId;
                Commission = config.Commission;
                CashPortName = config.CashPortName;
                CashBaudRate = config.CashBaudRate;
                FullScreen = config.FullScreen;
                CashCodeRequired = config.CashCodeRequired;
                WalletRequired = config.WalletRequired;
                UsePrinter = config.UsePrinter;

                return Error.Code.Sucess;
            }
            catch
            {
                return Error.Code.InvalidJsonFormat;
            }
        }
    }
}
