using Microsoft.Win32;
using System;
using System.Net;

namespace SessionMonitor.Common
{
    /// <summary>
    /// Listens for Session Switch event and raises an event to notify subscribers.
    /// </summary>
    public class SessionSwitchListener : IDisposable
    {
        private readonly string _computerName;
        private readonly string _userName;
        private SessionSwitchEventHandler _sseh;

        public event EventHandler<SessionSwitchedEventArgs> SessionSwitched;

        public SessionSwitchListener()
        {
            IPHostEntry localHost = Dns.GetHostEntry("localhost");
            _computerName = localHost != null ? localHost.HostName : Environment.MachineName;

            _userName = Environment.UserName;
        }

        private void SysEventsCheck(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.ConsoleConnect:
                case SessionSwitchReason.ConsoleDisconnect:
                case SessionSwitchReason.RemoteConnect:
                case SessionSwitchReason.RemoteDisconnect:
                case SessionSwitchReason.SessionLock:
                case SessionSwitchReason.SessionLogoff:
                case SessionSwitchReason.SessionLogon:
                case SessionSwitchReason.SessionUnlock:
                case SessionSwitchReason.SessionRemoteControl:
                    if (SessionSwitched != null)
                    {
                        var args = new SessionSwitchedEventArgs()
                        {
                            Reason = e.Reason,
                            ComputerName = _computerName,
                            UserName = _userName
                        };

                        SessionSwitched(this, args);
                    }
                    break;

                default:
                    throw new ApplicationException(string.Format($"Unexpected switch event encountered: {e.Reason}"));
            }
        }

        public void Run()
        {
            _sseh = new SessionSwitchEventHandler(SysEventsCheck);

            SystemEvents.SessionSwitch += _sseh;
        }

        public void Dispose()
        {
            SystemEvents.SessionSwitch -= _sseh;
        }
    }
}