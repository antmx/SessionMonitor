using SessionMonitor.Common;
using System;
using System.Configuration;
using System.ServiceProcess;

namespace SessionMonitor.WindowsSvc
{
    static class Program
    {
        internal static string LogName = "SessionMonitor.WindowsService";

        private static int _timerIntervalMs;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (!int.TryParse(ConfigurationManager.AppSettings["TimerIntervalSeconds"], out int timerIntervalSeconds)
                || timerIntervalSeconds < 1)
            {
                // Default to 5 seconds
                _timerIntervalMs = 5 * 1000;
            }
            else
            {
                // Convert to milliseconds
                _timerIntervalMs = timerIntervalSeconds * 1000;
            }

            log4net.LogManager.GetLogger(LogName).InfoFormat($"_timerIntervalMs={_timerIntervalMs}");

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
                new SessionService(_timerIntervalMs)
            };

            ServiceBase.Run(servicesToRun);
        }

        private static void RunConsole()
        {
            log4net.LogManager.GetLogger(LogName).Info("RunConsole");

            using (SessionServiceWrapper wrapper = new SessionServiceWrapper(_timerIntervalMs))
            {
                try
                {
                    wrapper.TestStart();
                    var s = Console.ReadLine();
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
