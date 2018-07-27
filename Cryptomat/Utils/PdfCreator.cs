using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;

namespace Cryptomat.Utils
{
    class PdfCreator
    {
        private const string filename = "check.pdf";
       
        private static XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
        private static XFont font = new XFont("Consolas", 16, XFontStyle.Regular, options);

        private const int startX = 10;
        private const int startY = 20;
        private const int lineHeight = 18;

        private static int x, y;

        public static void CreateCheck()
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Cryptomat check";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            ResetPosition();

            var time = DateTime.Now.ToString("dd.MM.yy HH:mm:ss");
            var year = DateTime.Now.Year;

            Println("=========Crypmat=========", gfx);
            Println($"Внесено   : { PaymentManager.Value } BYN", gfx);
            Println($"Зачислено : { PaymentManager.TotalSum} BTC", gfx);
            Println($"На счёт {PaymentManager.Wallet}", gfx);
            Println($"Время {time}", gfx);
            Println("-------------------------", gfx);
            Println($"(c) Cryptocode {year}", gfx);

            document.Save(filename);
            ResetPosition();
        }

        private static void Println(string s, XGraphics gfx)
        {
            gfx.DrawString(s, font, XBrushes.Black, x, y);
            y += lineHeight;
        }

        private static void ResetPosition()
        {
            x = startX;
            y = startY;
        }
    }
}
