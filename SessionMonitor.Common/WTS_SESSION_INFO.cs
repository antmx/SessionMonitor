using System;
using System.Runtime.InteropServices;

namespace SessionMonitor.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WTS_SESSION_INFO
    {
        public Int32 SessionID;

        [MarshalAs(UnmanagedType.LPStr)]
        public String pWinStationName;

        public WTS_CONNECTSTATE_CLASS State;
    }
}