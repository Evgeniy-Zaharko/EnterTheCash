using System;
using System.Windows;
using System.Windows.Threading;
using CashCode.Net;
using Cryptomat.Utils;
using Cryptomat.Pages;

namespace Cryptomat
{
    public partial class Payment
    {
        public Payment()
        {
            Loaded += OnContentRendered;
            PaymentManager.CashCode.BillReceived += UpdateBalanceHandler;
            InitializeComponent();
        }

        private void UpdateBalanceHandler (object sender, BillReceivedEventArgs e)
        {
            UpdateBalance();
        }

        private void UpdateBalance()
        {
            BalanceTb.Text = PaymentManager.Value + " р.";
            var value = CurrencesConverter.BynToBtc(PaymentManager.Value);
            ToPayTb.Text = $"Сумма: {value}";
            CommissionTb.Text = $"Коммиссия: {PaymentManager.Commission*100}%";
            FinalTb.Text = $"Будет зачислено: { PaymentManager.TotalSum }";

            if (PaymentManager.Value > 0)
                PayBt.IsEnabled = true;
        }


        private void OnContentRendered(object sender, RoutedEventArgs e)
        {
            if (!JsonConfig.CashCodeRequired)
            {
                Add5RubBt.Visibility = Visibility.Visible;
                Add10RubBt.Visibility = Visibility.Visible;
                Add5RubBt.IsEnabled = true;
                Add10RubBt.IsEnabled = true;
            }

            if (!JsonConfig.WalletRequired)
                PayBt.IsEnabled = true;

            try
            {
                PaymentManager.EnableCashCode();
            }
            catch
            {
                if (JsonConfig.CashCodeRequired)
                {
                    var err = new ErrorPage(Error.Code.CantLaunchCashCode);
                    NavigationService?.Navigate(err);
                }
                else
                {
                    BalanceTb.Text = "недоступно";
                }
            }
            
            Dispatcher.BeginInvoke(new Action(SetCoursesText),
                DispatcherPriority.ContextIdle, null);
        }

        private void SetCoursesText()
        {
            BynToUsdTb.Text = $"1 USD = {CurrencesConverter.BynUsdCourse:F4} BYN";
            UsdToBtcTb.Text = $"1 BTC = {CurrencesConverter.UsdBtcCourse:F2} USD";

            WalletTb.Text = $"Кошелёк {PaymentManager.Wallet}";
        }

        private void PayBt_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO : pay
            //PaymentManager.ResetValue();
            try
            {
                PaymentManager.DisableCashCode();
                var succPage = new Success();
                NavigationService?.Navigate(succPage);
            }
            catch
            {
                if (JsonConfig.CashCodeRequired)
                {
                    var err = new ErrorPage(Error.Code.CantDisableCashCode);
                    NavigationService?.Navigate(err);
                }
                else
                {
                    var succPage = new Success();
                    NavigationService?.Navigate(succPage);
                }
            }
        }


        // Experiemental
        private void Add5RubBt_Click(object sender, RoutedEventArgs e)
        {
            PaymentManager.SumulatePayment(5);
            UpdateBalance();
        }

        // Experiemental
        private void Add10RubBt_Click(object sender, RoutedEventArgs e)
        {
            PaymentManager.SumulatePayment(10);
            UpdateBalance();
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
