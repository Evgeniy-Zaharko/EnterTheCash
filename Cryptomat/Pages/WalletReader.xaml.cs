using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;
using Cryptomat.Utils;
using ZXing;


namespace Cryptomat
{
    public partial class WalletReader
    {
        private VideoCaptureDevice _videoSource;
        private BarcodeReader _reader;

        private delegate void SetStringDelegate(string parameter);

        private void SetResult(string result)
        {
            if (Dispatcher.CheckAccess())
            {
                WalletTb.Text = result;

                if (Wallets.VerifyWallet(result))
                {
                    ContinueBt.IsEnabled = true;
                    WalletTb.Foreground  = new SolidColorBrush(Colors.LimeGreen);
                }
                else
                {
                    ContinueBt.IsEnabled = false;
                    WalletTb.Foreground = new SolidColorBrush(Colors.DarkRed);
                }
            }
            else
            {
                Dispatcher.Invoke(new SetStringDelegate(SetResult), result);
            }
        }

        public WalletReader()
        {
            InitializeComponent();

            if (JsonConfig.WalletRequired)
            {
                Loaded += CreateCamera;
                Loaded += CreateReader;
                if (Application.Current.MainWindow != null)
                    Application.Current.MainWindow.Closing += MainWindowClosing;
            }
            else
            {
                Loaded += OnContentRendered;
            }
        }

        private void OnContentRendered(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(ContinueWithoutCamera),
                DispatcherPriority.ContextIdle, null);
        }

        private void ContinueWithoutCamera()
        {
            ContinueBt.IsEnabled = true;
            WalletTb.Text = "[без кошелька]";
        }

        private void CreateReader(object sender, RoutedEventArgs e)
        {
            _reader = new BarcodeReader
            {
                Options =
                {
                    PossibleFormats = new List<BarcodeFormat> {BarcodeFormat.QR_CODE}
                }
            };
        }

        private void CreateCamera(object sender, RoutedEventArgs e)
        {
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count < 1) return;

            _videoSource = new VideoCaptureDevice(videoDevices[JsonConfig.CameraId].MonikerString);
            _videoSource.NewFrame += NewFrame;
            _videoSource.Start();
        }

       
        /// <summary>
        /// Calls at each frame
        /// Reads the qr-code
        /// </summary>
        private void NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Image img = (Bitmap)eventArgs.Frame.Clone();
                var result = _reader.Decode((Bitmap)eventArgs.Frame.Clone());

                if (result != null)
                    SetResult(result.Text);   
                
                // Convert bitmap to ImageSource
                var ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                bi.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    QrImage.Source = bi;
                }));
            }
            catch (Exception){ /*ignored*/ }
        }

        private void DisableCamera()
        {
            if (_videoSource == null) return;
            _videoSource.SignalToStop();
            _videoSource.WaitForStop();
        }
        

        /// <summary>
        /// Stop videosource when we close the window
        /// </summary>
        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            DisableCamera();
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            DisableCamera();
            PaymentManager.Wallet = Wallets.ExtractWallet(WalletTb.Text);
            var pay = new Payment();
            NavigationService?.Navigate(pay);
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
