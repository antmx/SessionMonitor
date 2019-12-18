using SessionMonitor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static SessionMonitor.Common.TSManager;

namespace SessionMonitor.WindowsApp
{
    public partial class MainForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(Program.LogName);

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblOutput.Text = string.Empty;

            //InitSessionSwitchListener();

            //LoadSessionReaderInfo();
            //LoadTSManagerInfo();
            PrintRDPInfo();
        }

        private void InitSessionSwitchListener()
        {
            var listener = new SessionSwitchListener();

            listener.SessionSwitched += Listener_SessionSwitched;

            //listener.Run();

            lblOutput.Text += "Awaiting session switch event...";
        }

        private void PrintRDPInfo()
        {
            List<RdpSession> serverList = TSManager.ListRdpUsers();

            foreach (RdpSession item in serverList)
            {
                var line = $"> {item.UserName.Trim()} {item.ConnectionState} {item.Client} {item.SessionInfo.LogonTime}";
                log.Info(line);
                Console.WriteLine(line);
            }

        }

        //private void LoadTSManagerInfo()
        //{
        //    List<TSInfo> results = TSManager.ListUsers("localhost");

        //    var sb = new StringBuilder();

        //    if (results.Count() > 0)
        //    {
        //        sb.Append("\nCurrent terminal info:");
        //    }

        //    foreach (TSInfo item in results)
        //    {
        //        string line = string.Format($"UserName={item.UserName}, Domain={item.DomainName}, Connect State={item.ConnectState}, Client Name={item.ClientName}, Session ID={item.SessionId}, Win Station Name={item.WinStationName}, Logon Time={item.LogonTime}, Idle Time={item.IdleTime}");

        //        sb.Append("\n" + line);

        //        log.InfoFormat(line);
        //    }

        //    lblOutput.Text += sb.ToString();

        //}

        private void LoadSessionReaderInfo()
        {
            string[] validLogonTypes = { "Interactive", "Network", "RemoteInteractive" };
            string[] invalidLoginDomains = { "Window Manager", "Font Driver Host" };

            var reader = new SessionReader();

            IEnumerable<SessionInfo> sessions = reader.Read()
                .Where((s) => validLogonTypes.Contains(s.LogonType, StringComparer.OrdinalIgnoreCase)
                   && !invalidLoginDomains.Contains(s.LoginDomain, StringComparer.OrdinalIgnoreCase))
                .Distinct()
                .OrderBy((s) => s.LoginTime)
                .ThenBy((s) => s.Username);

            var sb = new StringBuilder();

            if (sessions.Count() > 0)
            {
                sb.Append("\nCurrent session info:");
            }

            foreach (SessionInfo session in sessions)
            {
                sb.Append($"\nUsername={session.Username}, LogonType={session.LogonType}, LogonServer={session.LogonServer}, LoginTime={session.LoginTime}");

                log.InfoFormat($"Username={session.Username}, LogonType={session.LogonType}, LogonServer={session.LogonServer}, LoginTime={session.LoginTime}");
            }

            lblOutput.Text += sb.ToString();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //LoadSessionReaderInfo();
            //LoadTSManagerInfo();
            PrintRDPInfo();
        }

        private void Listener_SessionSwitched(object sender, SessionSwitchedEventArgs e)
        {
            lblOutput.Text += string.Format($"\n{e.Reason} encountered at {DateTime.Now:HH:mm:ss}, on {e.ComputerName}, by {e.UserName}");

            //LoadSessionReaderInfo();
            //LoadTSManagerInfo();
            PrintRDPInfo();
        }
    }
}