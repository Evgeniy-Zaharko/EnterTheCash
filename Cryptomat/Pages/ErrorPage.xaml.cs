using System.Windows;
using System.Windows.Controls;
using Cryptomat.Utils;

namespace Cryptomat
{
    public partial class ErrorPage
    {
        private Utils.Error.Code errorCode;

        public ErrorPage(Utils.Error.Code errorCode)
        {
            this.errorCode = errorCode;
            Loaded += ShowError;
            InitializeComponent();
        }

        private void ShowError(object sender, RoutedEventArgs e)
        {
            ErrorTb.Text = errorCode.ToString();
        }
    }
}
