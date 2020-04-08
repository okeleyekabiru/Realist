using System;

namespace Realist.Data.Model
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string UserLocation { get; set; }
        public string UserIpHost { get; set; }
        public string BrowserId { get; set; }
        public  string DeviceName { get; set; }
        public  string DeviceImeI { get; set; }
        public string UserId { get; set; }
        public bool OsSuccess { get; set; }
        public string OsName { get; set; }
        public string OsPlatForm { get; set; }
        public string OsVersion { get; set; }
        public string OsShortName { get; set; }

    }
}