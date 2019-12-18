using System;

namespace SessionMonitor.Common
{
    public partial class TSManager
    {
        public class RdpSession : IEquatable<RdpSession>
        {
            public string UserName;
            public string Domain;
            public int SessionId;
            public string Client;
            public string Server;
            public WTS_CONNECTSTATE_CLASS ConnectionState;
            public WTSINFOW SessionInfo;

            public bool Equals(RdpSession other)
            {
                if (other == null) 
                    return false;

                return SessionId == other.SessionId 
                    && string.Equals(UserName, other.UserName)
                    && string.Equals(Domain, other.Domain)
                    && ConnectionState = other.ConnectionState
                    && SessionInfo.SessionId = other.SessionInfo.SessionId
                    && 
            }
        }
    }
}