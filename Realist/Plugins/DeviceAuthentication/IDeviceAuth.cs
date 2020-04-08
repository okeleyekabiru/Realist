using System;
using System.Collections.Generic;
using System.Text;
using Plugins.DeviceAuthentication;

namespace Plugins
{
 public   interface IDeviceAuth
 {
     string GetIMeiInfo();
     string GetCurrentUserIp();
     string GetBrowserId();
     bool VerifyIMei(string imei);
     string GetUserLocation(string location);
   DeviceInfo GetDeviceDetails();
 }
}
