using CashCode.Net;

namespace Cryptomat.Utils
{
    public class PaymentManager
    {
        public static CashCodeBillValidator CashCode;
        public static int Value { get; private set; }  // Amount of money
        public static bool CashCodeEnabled { get; private set; }
        public static double Commission { get; private set; }
        public static string Wallet { get; set; }
        public static double TotalSum { get => CurrencesConverter.BynToBtc(Value)*(1-Commission); }
        

        public static void Start()
        {
            Value = 0;
            CashCodeEnabled = false;
            CashCode = new CashCodeBillValidator(JsonConfig.CashPortName, JsonConfig.CashBaudRate);
            CashCode.BillReceived += OnBillReceived;

            Commission = JsonConfig.Commission;
        }

        private static void OnBillReceived(object sender, BillReceivedEventArgs e)
        {
            if (e.Status == BillRecievedStatus.Accepted)  // On success
            {
                Value += e.Value;
            }
        }

        public static void ResetValue()
        {
            Value = 0;
        }

        public static void EnableCashCode()
        {
            CashCode.StartListening();
            CashCodeEnabled = true;
        }

        public static void DisableCashCode()
        {
            CashCode.StopListening();
            CashCodeEnabled = false;
        }

        public static void SumulatePayment(int amount)
        {
            Value += amount;
        }
    }
}
