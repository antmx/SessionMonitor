using log4net;
using SessionMonitor.Common;
using System;
using System.Collections.Generic;//
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static SessionMonitor.Common.TSManager;

namespace SessionMonitor.WindowsSvc
{
    public partial class SessionService : ServiceBase
    {
        private static readonly ILog _log = LogManager.GetLogger(Program.LogName);
        //private ISessionSwitchListener _sessionSwitchListener;
        private Timer _timer;

        public SessionService(/*ISessionSwitchListener sessionSwitchListener*/ int timerIntervalMs)
        {
            InitializeComponent();

            //_sessionSwitchListener = sessionSwitchListener;

            _timer = new Timer(timerIntervalMs);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ReportSessionInfo();
        }

        /// <summary>
        /// Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            _log.Info("Starting SessionService");

            //_sessionSwitchListener.SessionSwitched += _sessionSwitchListener_SessionSwitched;

            ReportSessionInfo();
        }

        /// <summary>
        /// Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            _log.Info("Stopping SessionService");

            //_sessionSwitchListener.SessionSwitched -= _sessionSwitchListener_SessionSwitched;
        }

        //private void _sessionSwitchListener_SessionSwitched(object sender, SessionSwitchedEventArgs e)
        //{
        //    _log.InfoFormat($"SessionSwitched: {e.Reason} encountered at {DateTime.Now:HH:mm:ss}, on {e.ComputerName}, by {e.UserName}");

        //    ReportSessionInfo();
        //}

        private void ReportSessionInfo() {

            //foreach (SessionInfo session in FetchSessionInfo())
            //{
            //    _log.InfoFormat($"Username={session.Username}, LogonType={session.LogonType}, LogonServer={session.LogonServer}, LoginDomain={session.LoginDomain}, LoginTime={session.LoginTime}");
            //    // todo make call to Web API
            //}

            foreach (RdpSession item in TSManager.ListRdpUsers("localhost"))
            {
                _log.InfoFormat($"User={item.UserName.Trim()}, Connect State={item.ConnectionState}, Client={item.Client}, Logon Time={item.SessionInfo.LogonTime}, State={item.SessionInfo.State}, Win Station={item.SessionInfo.WinStationName}");

                // todo make call to Web API
            }
        }

        //private IEnumerable<SessionInfo> FetchSessionInfo()
        //{
        //    string[] validLogonTypes = { "Interactive", "Network", "RemoteInteractive" };
        //    string[] invalidLoginDomains = { "Window Manager", "Font Driver Host" };
        //    string[] invalidUsernames = { "ANONYMOUS LOGON" };

        //    var reader = new SessionReader();

        //    IEnumerable<SessionInfo> sessions = reader.Read()
        //        .Where((s) => validLogonTypes.Contains(s.LogonType, StringComparer.OrdinalIgnoreCase)
        //           && !invalidLoginDomains.Contains(s.LoginDomain, StringComparer.OrdinalIgnoreCase)
        //           && !invalidUsernames.Contains(s.Username, StringComparer.OrdinalIgnoreCase))
        //        .Distinct()
        //        .OrderBy((s) => s.LoginTime)
        //        .ThenBy((s) => s.Username);

        //    return sessions;
        //}

    }
}
