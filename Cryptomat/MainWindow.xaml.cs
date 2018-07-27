using System.Windows;
using Cryptomat.Utils;

namespace Cryptomat
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var code = JsonConfig.Init();

            if (code == Error.Code.Sucess)
            {
                if(JsonConfig.FullScreen)
                    WindowStyle = WindowStyle.None;
                CurrencesConverter.Start();
                PaymentManager.Start();

                var startPage = new Init();
                MainFrame.NavigationService.Navigate(startPage);
            }
            else
            {
                var errorPage = new ErrorPage(code);
                MainFrame.NavigationService.Navigate(errorPage);
            }
        }
    }
}
