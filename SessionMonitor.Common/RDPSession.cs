using System;

namespace SessionMonitor.Common
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
            {
                return false;
            }

            return SessionId == other.SessionId
                && string.Equals(UserName, other.UserName)
                && string.Equals(Domain, other.Domain)
                && string.Equals(Client, other.Client)
                && string.Equals(Server, other.Server)
                && ConnectionState == other.ConnectionState
                && SessionInfo.Equals(other.SessionInfo);
        }
    }
}
