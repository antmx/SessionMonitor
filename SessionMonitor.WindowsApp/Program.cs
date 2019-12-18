using System;
using System.Windows.Forms;

namespace SessionMonitor.WindowsApp
{
    static class Program
    {
        internal static string LogName = "SessionMonitor.WindowsApp";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}