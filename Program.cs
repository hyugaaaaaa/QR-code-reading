using System.Text; // <-- usingディレクティブを追加

namespace QRtoCSVSystem
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Shift-JISエンコーディングのサポートを登録 (ここから追記)
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new FormQRtoCSV());
        }
    }
}