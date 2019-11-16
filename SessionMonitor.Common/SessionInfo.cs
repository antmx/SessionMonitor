using System;

namespace SessionMonitor.Common
{
    public class SessionInfo
    {
        #region SECURITY_LOGON_SESSION_DATA

        public string Sid;
        public string AuthenticationPackage;
        public string DnsDomainName;
        public string LoginDomain;
        public string LogonServer;

        /// <summary>
        /// A user account name (sometimes referred to as the user logon name) and a domain name identifying the domain in which the user account is located.
        /// </summary>
        //public string UserPrincipleName;

        public string Username;

        public string LogonType;

        /// <summary>
        /// A Terminal Services session identifier. This member may be zero.
        /// </summary>
        public UInt32 Session;

        public DateTime LoginTime;

        public DateTime LogoffTime;

        #endregion

        /// <summary>
        /// Overridden so Linq.Distinct works.
        /// </summary>
        /// <param name="other">Another object to compare a</param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var otherSessionInfo = other as SessionInfo;

            if (otherSessionInfo == null)
            {
                return false;
            }

            if (!string.Equals(Sid, otherSessionInfo.Sid, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(LogonType, otherSessionInfo.LogonType, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Sid.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format($"DnsDomainName={DnsDomainName}, Username={Username}, Sid={Sid}, AuthenticationPackage={AuthenticationPackage}, LoginDomain={LoginDomain}, LogonServer={LogonServer}, LogonType={LogonType}, LoginTime={LoginTime}, LogoffTime={LogoffTime}, Session={Session}");
        }

    }
}