using System;

namespace SessionMonitor.Common
{
    public interface ISessionSwitchListener
    {
        event EventHandler<SessionSwitchedEventArgs> SessionSwitched;

        void Dispose();
        //void Run();
    }
}