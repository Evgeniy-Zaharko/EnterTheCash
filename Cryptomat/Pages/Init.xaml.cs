using System.Windows;

namespace Cryptomat
{
    public partial class Init
    {
        public Init()
        {
            InitializeComponent();
        }

        private void GoToQr(object sender, RoutedEventArgs e)
        {
            var rw = new WalletReader();
            NavigationService?.Navigate(rw);
        }
    }
}
