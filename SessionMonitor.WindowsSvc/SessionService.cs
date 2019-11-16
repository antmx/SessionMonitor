using SessionMonitor.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SessionMonitor.WindowsSvc
{
    public partial class SessionService : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SessionService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        private void InitSessionSwitchListener()
        {
            var listener = new SessionSwitchListener();

            listener.SessionSwitched += Listener_SessionSwitched;

            listener.Run();
        }

        private void Listener_SessionSwitched(object sender, SessionSwitchedEventArgs e)
        {
            log.InfoFormat($"SessionSwitched: {e.Reason} encountered at {DateTime.Now:HH:mm:ss}, on {e.ComputerName}, by {e.UserName}");

            foreach (SessionInfo session in FetchSessionInfo())
            {
                log.InfoFormat($"Username={session.Username}, LogonType={session.LogonType}, LogonServer={session.LogonServer}, LoginDomain={session.LoginDomain}, LoginTime={session.LoginTime}");

                // todo make call to Web API
            }
        }

        private List<SessionInfo> FetchSessionInfo()
        {
            string[] validLogonTypes = { "Interactive", "Network", "RemoteInteractive" };
            string[] invalidLoginDomains = { "Window Manager", "Font Driver Host" };

            var reader = new SessionReader();

            IEnumerable<SessionInfo> sessions = reader.Read()
                .Where((s) => validLogonTypes.Contains(s.LogonType, StringComparer.OrdinalIgnoreCase)
                   && !invalidLoginDomains.Contains(s.LoginDomain, StringComparer.OrdinalIgnoreCase))
                .Distinct()
                .OrderBy((s) => s.Username)
                .ThenBy((s) => s.LoginTime);

            List<SessionInfo> results = sessions.ToList();

            return results;
        }
    }
}
