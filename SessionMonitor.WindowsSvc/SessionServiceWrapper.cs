using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SessionMonitor.Common;

namespace SessionMonitor.WindowsSvc
{
    internal class SessionServiceWrapper : SessionService
    {
        public SessionServiceWrapper(int timerIntervalMs) : base(timerIntervalMs)
        {
        }

        public void TestStart()
        {
            base.OnStart(new string[] { });
        }

        public void TestStop()
        {
            base.OnStop();
        }
    }
}