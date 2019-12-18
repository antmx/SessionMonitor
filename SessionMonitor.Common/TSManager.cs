using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SessionMonitor.Common
{
    public partial class TSManager
    {
        #region DllImports

        [DllImport("wtsapi32.dll")]
        static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] String pServerName);

        [DllImport("wtsapi32.dll")]
        static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("wtsapi32.dll")]
        static extern Int32 WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] Int32 Reserved,
            [MarshalAs(UnmanagedType.U4)] Int32 Version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref Int32 pCount);

        [DllImport("wtsapi32.dll")]
        static extern void WTSFreeMemory(IntPtr pMemory);

        [DllImport("Wtsapi32.dll")]
        static extern bool WTSQuerySessionInformation(
            IntPtr hServer,
            int sessionId,
            WTS_INFO_CLASS wtsInfoClass,
            out IntPtr ppBuffer,
            out uint pBytesReturned);

        #endregion

        public static IntPtr OpenServer(String name)
        {
            IntPtr server = WTSOpenServer(name);
            return server;
        }

        public static void CloseServer(IntPtr serverHandle)
        {
            WTSCloseServer(serverHandle);
        }

        //public static List<TSInfo> ListUsers(String serverName)
        //{
        //    IntPtr serverHandle = IntPtr.Zero;
        //    List<TSInfo> resultList = new List<TSInfo>();
        //    serverHandle = OpenServer(serverName);
        //    //WTSINFOA wtsinfo = new WTSINFOA();

        //    try
        //    {
        //        IntPtr sessionInfoPtr = IntPtr.Zero;
        //        IntPtr userPtr = IntPtr.Zero;
        //        IntPtr domainPtr = IntPtr.Zero;
        //        IntPtr connectStatePtr = IntPtr.Zero;
        //        IntPtr clientNamePtr = IntPtr.Zero;
        //        IntPtr sessionIdPtr = IntPtr.Zero;
        //        IntPtr idleTimePtr = IntPtr.Zero;
        //        IntPtr logonTimePtr = IntPtr.Zero;
        //        IntPtr winStationNamePtr = IntPtr.Zero;
        //        IntPtr wtsInfoPtr = IntPtr.Zero;
        //        Int32 sessionCount = 0;
        //        Int32 retVal = WTSEnumerateSessions(serverHandle, 0, 1, ref sessionInfoPtr, ref sessionCount);
        //        Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
        //        IntPtr currentSession = sessionInfoPtr;
        //        uint bytesReturned = 0;

        //        if (retVal != 0)
        //        {
        //            for (int i = 0; i < sessionCount; i++)
        //            {
        //                WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((IntPtr)currentSession, typeof(WTS_SESSION_INFO));
        //                currentSession += dataSize;

        //                WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSUserName, out userPtr, out bytesReturned);
        //                WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSDomainName, out domainPtr, out bytesReturned);
        //                WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSConnectState, out connectStatePtr, out bytesReturned);
        //                WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSClientName, out clientNamePtr, out bytesReturned);
        //                WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSSessionId, out sessionIdPtr, out bytesReturned);
        //                WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSIdleTime, out idleTimePtr, out bytesReturned);
        //                WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSLogonTime, out logonTimePtr, out bytesReturned);
        //                WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSWinStationName, out winStationNamePtr, out bytesReturned);
        //                WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSSessionInfo, out sessionInfoPtr, out bytesReturned);
        //                WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSSessionInfo, out wtsInfoPtr, out bytesReturned);

        //                wtsinfo = (WTSINFOA)Marshal.PtrToStructure(wtsInfoPtr, typeof(WTSINFOA));

        //                //Console.WriteLine("Domain and User: " + Marshal.PtrToStringAnsi(domainPtr) + "\\" + Marshal.PtrToStringAnsi(userPtr));

        //                var tsInfo = new TSInfo
        //                {
        //                    UserName = Marshal.PtrToStringAnsi(userPtr),
        //                    DomainName = Marshal.PtrToStringAnsi(domainPtr),
        //                    ConnectState = Marshal.PtrToStringAnsi(connectStatePtr),
        //                    ClientName = Marshal.PtrToStringAnsi(clientNamePtr),
        //                    SessionId = Marshal.ReadInt32(sessionIdPtr),
        //                    State = si.State.ToString(),
        //                    WinStationName = Marshal.PtrToStringAnsi(winStationNamePtr),
        //                    //IdleTime = Marshal.ReadInt32(idleTimePtr),
        //                    //LogonTime = Marshal.ReadInt32(logonTimePtr)
        //                };

        //                if (string.IsNullOrEmpty(tsInfo.UserName) && string.IsNullOrEmpty(tsInfo.DomainName))
        //                {
        //                    continue;
        //                }

        //                resultList.Add(tsInfo);

        //                WTSFreeMemory(userPtr);
        //                WTSFreeMemory(domainPtr);
        //            }

        //            WTSFreeMemory(sessionInfoPtr);
        //        }

        //        return resultList;
        //    }
        //    finally
        //    {
        //        CloseServer(serverHandle);
        //    }

        //}

        public static List<RdpSession> ListRdpUsers()
        {
            return ListRdpUsers(null);
        }

        public static List<RdpSession> ListRdpUsers(String serverName)
        {
            List<RdpSession> List = new List<RdpSession>();

            IntPtr serverHandle = IntPtr.Zero;
            List<String> resultList = new List<string>();
            serverHandle = OpenServer(serverName);

            IntPtr sessionInfoPtr = IntPtr.Zero;
            IntPtr clientNamePtr = IntPtr.Zero;
            IntPtr wtsInfoPtr = IntPtr.Zero;
            //IntPtr clientDisplayPtr = IntPtr.Zero;
            IntPtr idleTimePtr = IntPtr.Zero;
            IntPtr logonTimePtr = IntPtr.Zero;

            try
            {

                Int32 sessionCount = 0;
                Int32 retVal = WTSEnumerateSessions(serverHandle, 0, 1, ref sessionInfoPtr, ref sessionCount);
                Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                Int32 currentSession = (int)sessionInfoPtr;
                uint bytesReturned = 0;

                if (retVal != 0)
                {
                    for (int i = 0; i < sessionCount; i++)
                    {
                        WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((IntPtr)currentSession, typeof(WTS_SESSION_INFO));
                        currentSession += dataSize;

                        WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSClientName, out clientNamePtr, out bytesReturned);
                        WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSSessionInfo, out wtsInfoPtr, out bytesReturned);
                        WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSIdleTime, out idleTimePtr, out bytesReturned);
                        WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSLogonTime, out logonTimePtr, out bytesReturned);

                        var wtsinfo = (WTSINFOW)Marshal.PtrToStructure(wtsInfoPtr, typeof(WTSINFOW));

                        RdpSession temp = new RdpSession
                        {
                            Client = Marshal.PtrToStringAnsi(IntPtr.Zero),
                            Server = serverName,
                            UserName = wtsinfo.UserName,
                            Domain = wtsinfo.Domain,
                            ConnectionState = si.State,
                            SessionId = si.SessionID,
                            SessionInfo = wtsinfo
                        };

                        if (!string.IsNullOrEmpty(temp.UserName))
                        {
                            List.Add(temp);
                        }

                        WTSFreeMemory(IntPtr.Zero);
                        WTSFreeMemory(wtsInfoPtr);
                    }

                    WTSFreeMemory(sessionInfoPtr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                CloseServer(serverHandle);
            }

            return List;
        }
    }
}