using System.Text.RegularExpressions;

namespace Cryptomat.Utils
{
    class Wallets
    {
        public static bool VerifyWallet(string wallet)
        {
            var checkedWallet = ExtractWallet(wallet);

            if (checkedWallet.Length > 50 || checkedWallet.Length < 20)
                return false;
            return Regex.IsMatch(checkedWallet, @"^[a-zA-Z0-9]+$");
        }

        public static string ExtractWallet(string data)
        {
            if (!data.Contains(":"))
                return "";
            return data.Substring(data.IndexOf(":")+1);
        }
    }
}
