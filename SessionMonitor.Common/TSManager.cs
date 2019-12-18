﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SessionMonitor.Common
{
    public partial class TSManager
    {
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

        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_SESSION_INFO
        {
            public Int32 SessionID;

            [MarshalAs(UnmanagedType.LPStr)]
            public String pWinStationName;

            public WTS_CONNECTSTATE_CLASS State;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WTSINFOW : IEquatable<WTSINFOW>
        {
            public bool Equals(WTSINFOW other)
            {
                
                if(SessionId == other.SessionId
            public int IncomingBytes;
            public int OutgoingBytes;
            public int IncomingFrames;
            public int OutgoingFrames;
            public int IncomingCompressedBytes;
            public int OutgoingCompressedBytes;)

            }

            public const int WINSTATIONNAME_LENGTH = 32;
            public const int DOMAIN_LENGTH = 17;
            public const int USERNAME_LENGTH = 20;
            public WTS_CONNECTSTATE_CLASS State;
            public int SessionId;
            public int IncomingBytes;
            public int OutgoingBytes;
            public int IncomingFrames;
            public int OutgoingFrames;
            public int IncomingCompressedBytes;
            public int OutgoingCompressedBytes;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = WINSTATIONNAME_LENGTH)]
            public byte[] WinStationNameRaw;
            public string WinStationName
            {
                get
                {
                    return Encoding.ASCII.GetString(WinStationNameRaw).TrimEnd('\0');
                }
            }

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DOMAIN_LENGTH)]
            public byte[] DomainRaw;
            public string Domain
            {
                get
                {
                    return Encoding.ASCII.GetString(DomainRaw).TrimEnd('\0');
                }
            }

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = USERNAME_LENGTH + 1)]
            public byte[] UserNameRaw;
            public string UserName
            {
                get
                {
                    return Encoding.ASCII.GetString(UserNameRaw).TrimEnd('\0');
                }
            }

            public long ConnectTimeUTC;
            public DateTime ConnectTime
            {
                get
                {
                    return DateTime.FromFileTimeUtc(ConnectTimeUTC);
                }
            }

            public long DisconnectTimeUTC;
            public DateTime DisconnectTime
            {
                get
                {
                    return DateTime.FromFileTimeUtc(DisconnectTimeUTC);
                }
            }

            public long LastInputTimeUTC;
            public DateTime LastInputTime
            {
                get
                {
                    return DateTime.FromFileTimeUtc(LastInputTimeUTC);
                }
            }

            public long IdleTimeUTC;
            public DateTime IdleTime
            {
                get
                {
                    return DateTime.FromFileTimeUtc(IdleTimeUTC);
                }
            }

            public long LogonTimeUTC;
            public DateTime LogonTime
            {
                get
                {
                    return DateTime.FromFileTimeUtc(LogonTimeUTC);
                }
            }

            public long CurrentTimeUTC;
            public DateTime CurrentTime
            {
                get
                {
                    //return DateTime.FromFileTimeUtc(CurrentTimeUTC);
                    return DateTime.FromBinary(CurrentTimeUTC);
                }
            }
        }

        public enum WTS_INFO_CLASS : int
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType,
            WTSIdleTime = 17,
            WTSLogonTime = 18,
            WTSIncomingBytes = 19,
            WTSOutgoingBytes = 20,
            WTSIncomingFrames = 21,
            WTSOutgoingFrames = 22,
            WTSClientInfo = 23,
            WTSSessionInfo = 24,
            WTSSessionInfoEx = 25,
            WTSConfigInfo = 26,
            WTSValidationInfo = 27,
            WTSSessionAddressV4 = 28,
            WTSIsRemoteSession = 29
        }

        public enum WTS_CONNECTSTATE_CLASS : int
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

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