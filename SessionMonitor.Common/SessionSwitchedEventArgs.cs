using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionMonitor.Common
{
    public class SessionSwitchedEventArgs
    {
        public SessionSwitchReason Reason { get; set; }

        public string ComputerName { get; set; }

        public string UserName { get; set; }
    }
}