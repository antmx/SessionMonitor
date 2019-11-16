using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace SessionMonitor.Common
{
    public class SessionReader
    {
        #region DLL imports

        [DllImport("Secur32.dll", SetLastError = false)]
        private static extern uint LsaFreeReturnBuffer(IntPtr buffer);

        [DllImport("Secur32.dll", SetLastError = false)]
        private static extern uint LsaEnumerateLogonSessions(out UInt64 LogonSessionCount, out IntPtr LogonSessionList);

        [DllImport("Secur32.dll", SetLastError = false)]
        private static extern uint LsaGetLogonSessionData(IntPtr luid, out IntPtr ppLogonSessionData);

        [StructLayout(LayoutKind.Sequential)]
        private struct LSA_UNICODE_STRING
        {
            public UInt16 Length;
            public UInt16 MaximumLength;
            public IntPtr buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LUID
        {
            public UInt32 LowPart;
            public UInt32 HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SECURITY_LOGON_SESSION_DATA
        {
            public UInt32 Size;
            public LUID LoginID;
            public LSA_UNICODE_STRING Username;
            public LSA_UNICODE_STRING LoginDomain;
            public LSA_UNICODE_STRING AuthenticationPackage;
            public UInt32 LogonType;
            public UInt32 Session;
            public IntPtr PSiD;
            public UInt64 LoginTime;
            public UInt64 LogoffTime;
            public LSA_UNICODE_STRING LogonServer;
            public LSA_UNICODE_STRING DnsDomainName;
            public LSA_UNICODE_STRING Upn;
        }
        private enum SECURITY_LOGON_TYPE : uint
        {
            UndefinedLogonType = 0,
            Interactive = 2,    //The security principal is logging on interactively.
            Network,        //The security principal is logging using a network.
            Batch,          //The logon is for a batch process.
            Service,        //The logon is for a service account.
            Proxy,          //Not supported.
            Unlock,         //The logon is an attempt to unlock a workstation.
            NetworkCleartext,   //The logon is a network logon with cleartext credentials.
            NewCredentials,     // Allows the caller to clone its current token and specify new credentials for outbound connections.
            RemoteInteractive,  // A terminal server session that is both remote and interactive.
            CachedInteractive,  // Attempt to use the cached credentials without going out across the network.
            CachedRemoteInteractive, // Same as RemoteInteractive, except used internally for auditing purposes.
            CachedUnlock      // The logon is an attempt to unlock a workstation.
        }

        #endregion

        public IList<SessionInfo> Read()
        {
            DateTime systime = new DateTime(1601, 1, 1, 0, 0, 0, 0); //win32 systemdate
            UInt64 sessionCount;
            IntPtr luidPtr = IntPtr.Zero;
            LsaEnumerateLogonSessions(out sessionCount, out luidPtr);  //gets an array of pointers to LUIDs
            IntPtr iter = luidPtr;      //set the pointer to the start of the array

            List<SessionInfo> results = new List<SessionInfo>();

            for (ulong sessionIdx = 0; sessionIdx < sessionCount; sessionIdx++)   //for each pointer in the array
            {
                IntPtr sessionData;
                LsaGetLogonSessionData(iter, out sessionData);
                SECURITY_LOGON_SESSION_DATA data = (SECURITY_LOGON_SESSION_DATA)Marshal.PtrToStructure(sessionData, typeof(SECURITY_LOGON_SESSION_DATA));

                //if we have a valid logon
                if (data.PSiD != IntPtr.Zero)
                {
                    //extract some useful information from the session data struct
                    SecurityIdentifier sid = new SecurityIdentifier(data.PSiD); //get the security identifier for further use
                    string authpackage = Marshal.PtrToStringUni(data.AuthenticationPackage.buffer).Trim();    //authentication package
                    string dnsDomainName = Marshal.PtrToStringUni(data.DnsDomainName.buffer).Trim();
                    string loginDomain = Marshal.PtrToStringUni(data.LoginDomain.buffer).Trim();    //domain for this account  
                    string logonServer = Marshal.PtrToStringUni(data.LogonServer.buffer).Trim();    //login server for this ???
                    string logonType = ((SECURITY_LOGON_TYPE)data.LogonType).ToString(); // the type of logon requested by a logon process
                    //string userPrincipleName = Marshal.PtrToStringUni(data.Upn.buffer).Trim();      //get the User Principle Name
                    string username = Marshal.PtrToStringUni(data.Username.buffer).Trim();      //get the account username
                    DateTime loginTime = systime.AddTicks((long)data.LoginTime);                  //get the datetime the session was logged in
                    //DateTime logoffTime = systime.AddTicks((long)data.LogoffTime);                  //get the datetime the session was logged off
                    DateTime logoffTime = new DateTime((long)data.LogoffTime);                  //get the datetime the session was logged off

                    //do something with the extracted data
                    var sessionInfo = new SessionInfo
                    {
                        AuthenticationPackage = authpackage,
                        DnsDomainName = dnsDomainName,
                        LoginDomain = loginDomain,
                        LogonServer = logonServer,
                        LoginTime = loginTime,
                        LogoffTime = logoffTime,
                        LogonType = logonType,
                        Session = 0, // todo
                        Sid = sid.Value,
                        //UserPrincipleName = userPrincipleName,
                        Username = username
                    };

                    results.Add(sessionInfo);

                    //do
                    //{

                    //} while (true);

                }

                iter = (IntPtr)((int)iter + Marshal.SizeOf(typeof(LUID)));  //move the pointer forward

                LsaFreeReturnBuffer(sessionData);   //free the SECURITY_LOGON_SESSION_DATA memory in the struct
            }

            LsaFreeReturnBuffer(luidPtr);   //free the array of LUIDs

            return results;
        }

    }
}