using Microsoft.Win32;
using System;
using System.Net;

namespace SessionMonitor.Common
{
    /// <summary>
    /// Listens for Session Switch event and raises an event to notify subscribers.
    /// </summary>
    public class SessionSwitchListener : IDisposable, ISessionSwitchListener
    {
        private readonly string _computerName;
        private readonly string _userName;
        //private SessionSwitchEventHandler _sseh;

        /// <summary>
        /// Notifies listeners that a SessionSwitch event occurs following a change of logged-in user.
        /// </summary>
        public event EventHandler<SessionSwitchedEventArgs> SessionSwitched;

        public SessionSwitchListener()
        {
            _computerName = GetComputerName();
            _userName = Environment.UserName;
            //_sseh = new SessionSwitchEventHandler(SysEventsCheck);
            //SystemEvents.SessionSwitch += _sseh;
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;

            SystemEvents.SessionEnded += SystemEvents_SessionEnded;
        }

        private void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e)
        {
            if (SessionSwitched != null)
            {
                var args = new SessionSwitchedEventArgs()
                {
                    Reason = SessionSwitchReason.SessionLogoff,// e.Reason,
                    ComputerName = _computerName,
                    UserName = _userName
                };

                SessionSwitched(this, args);
            }
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
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

        //public void Run()
        //{
        //    _sseh = new SessionSwitchEventHandler(SysEventsCheck);

        //    SystemEvents.SessionSwitch += _sseh;
        //}

        public void Dispose()
        {
            //SystemEvents.SessionSwitch -= _sseh;
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
        }

        private string GetComputerName()
        {
            IPHostEntry localHost = Dns.GetHostEntry("localhost");

            return localHost != null ? localHost.HostName : Environment.MachineName;
        }

        //private void SysEventsCheck(object sender, SessionSwitchEventArgs e)
        //{
        //    switch (e.Reason)
        //    {
        //        case SessionSwitchReason.ConsoleConnect:
        //        case SessionSwitchReason.ConsoleDisconnect:
        //        case SessionSwitchReason.RemoteConnect:
        //        case SessionSwitchReason.RemoteDisconnect:
        //        case SessionSwitchReason.SessionLock:
        //        case SessionSwitchReason.SessionLogoff:
        //        case SessionSwitchReason.SessionLogon:
        //        case SessionSwitchReason.SessionUnlock:
        //        case SessionSwitchReason.SessionRemoteControl:
        //            if (SessionSwitched != null)
        //            {
        //                var args = new SessionSwitchedEventArgs()
        //                {
        //                    Reason = e.Reason,
        //                    ComputerName = _computerName,
        //                    UserName = _userName
        //                };

        //                SessionSwitched(this, args);
        //            }
        //            break;

        //        default:
        //            throw new ApplicationException(string.Format($"Unexpected switch event encountered: {e.Reason}"));
        //    }
        //}
    }
}
