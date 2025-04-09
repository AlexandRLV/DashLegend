using System;

namespace Framework
{
    public static class TimeUtils
    {
        public static int Timestamp => (int)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;
    }
}