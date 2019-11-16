using SessionMonitor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SessionMonitor.WindowsApp
{
    public partial class MainForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblOutput.Text = string.Empty;

            InitSessionSwitchListener();

            LoadSessionReaderInfo();
        }

        private void InitSessionSwitchListener()
        {
            var listener = new SessionSwitchListener();

            listener.SessionSwitched += Listener_SessionSwitched;

            listener.Run();

            lblOutput.Text += "Awaiting session switch event...";
        }

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

            if (sessions.Count() > 0) {
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
            LoadSessionReaderInfo();
        }

        private void Listener_SessionSwitched(object sender, SessionSwitchedEventArgs e)
        {
            lblOutput.Text += string.Format($"\n{e.Reason} encountered at {DateTime.Now:HH:mm:ss}, on {e.ComputerName}, by {e.UserName}");

            LoadSessionReaderInfo();
        }
    }
}