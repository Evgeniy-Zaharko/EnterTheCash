using Cryptomat.Utils;
using System.Windows;
using System.Windows.Controls;

namespace Cryptomat.Pages
{
    /// <summary>
    /// Interaction logic for Success.xaml
    /// </summary>
    public partial class Success : Page
    {
        public Success()
        {
            InitializeComponent();
            Utils.PdfCreator.CreateCheck();
            PaymentManager.ResetValue();
        }

        private void PrintNoBt_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        private void PrintYesBt_Click(object sender, RoutedEventArgs e)
        {
            if(JsonConfig.UsePrinter)
                Printer.PrintCheck();
            Exit();
        }

        private void Exit()
        {
            var init = new Init();
            NavigationService?.Navigate(init);
        }
    }
}
