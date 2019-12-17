using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionMonitor.Common
{
    public class TSInfo
    {
        public string UserName { get; set; }
        public string DomainName { get; set; }
        public string ConnectState { get; set; }
        public string State { get; set; }
        public string ClientName { get; set; }
        public int SessionId { get; set; }
        public int IdleTime { get; set; }
        public int LogonTime { get; set; }
        public string WinStationName { get; set; }
    }
}
