using SessionMonitor.Common;
using System;
using System.ServiceProcess;

namespace SessionMonitor.WindowsSvc
{
    static class Program
    {
        internal static string LogName = "SessionMonitor.WindowsService";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0 && string.Equals(args[0], "console", StringComparison.OrdinalIgnoreCase))
            {
                RunConsole();
            }
            else
            {
                RunSvc();
            }
        }

        private static void RunSvc()
        {
            log4net.LogManager.GetLogger(LogName).Info("RunSvc");

            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                new SessionService(new SessionSwitchListener())
            };

            ServiceBase.Run(servicesToRun);
        }

        private static void RunConsole()
        {
            log4net.LogManager.GetLogger(LogName).Info("RunConsole");

            using (SessionServiceWrapper wrapper = new SessionServiceWrapper(new SessionSwitchListener()))
            {
                try
                {
                    wrapper.TestStart();
                    Console.ReadLine();
                    wrapper.TestStop();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                    Console.ReadLine();
                }
            }
        }
    }
}
