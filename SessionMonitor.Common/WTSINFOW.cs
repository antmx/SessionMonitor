using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SessionMonitor.Common
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WTSINFOW : IEquatable<WTSINFOW>
    {
        public bool Equals(WTSINFOW other)
        {
            return SessionId == other.SessionId
                && IncomingBytes == other.IncomingCompressedBytes
                && OutgoingBytes == other.OutgoingBytes
                && IncomingFrames == other.IncomingFrames
                && OutgoingFrames == other.OutgoingFrames
                && IncomingCompressedBytes == other.IncomingCompressedBytes
                && OutgoingCompressedBytes == other.OutgoingCompressedBytes
                && State == other.State;
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
}
