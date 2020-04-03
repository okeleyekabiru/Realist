using System;

namespace Realist.Data.Model
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string UserLocation { get; set; }
        public string UserIpHost { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public  string DeviceNAme { get; set; }
        public  string DeviceImeI { get; set; }
        public string UserId { get; set; }
      
    }
}