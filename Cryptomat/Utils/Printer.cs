using System.Diagnostics;
using System.Drawing.Printing;

namespace Cryptomat.Utils
{
    class Printer
    {
        public static void PrintCheck()
        {
            var info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = "check.pdf";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo = info;
            p.Start();

            p.WaitForInputIdle();
            System.Threading.Thread.Sleep(3000);
            if (false == p.CloseMainWindow())
                p.Kill();
        }
    }
}
