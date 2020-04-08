

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using DeviceDetectorNET;
using DeviceDetectorNET.Cache;
using Microsoft.AspNetCore.Http;

namespace Plugins.DeviceAuthentication
{
  public  class DeviceAuthentication:IDeviceAuth
    {
        private readonly IHttpContextAccessor _httpContext;

        public DeviceAuthentication(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }


        public string GetIMeiInfo()
        {
        return PhoneInfo.GetIMEI();
        }

        public string GetCurrentUserIp()
        {
            return _httpContext.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }

        public string GetBrowserId()
        {
            return _httpContext.HttpContext?.Request.Headers["User-Agent"].ToString();
        }

        public bool VerifyIMei(string imei)
        {
            long newImei;
            var stringImei = long.TryParse(imei, out newImei);
            if (stringImei)
            {
                return ConfirmIMei.isValidIMEI(newImei);

            }

            return false;

        }

        public string GetUserLocation(string location)
        {

            if (string.IsNullOrEmpty(location))
            {
                return RegionInfo.CurrentRegion.DisplayName;
            }

            return location;
        }

        public DeviceInfo GetDeviceDetails()
        {
            // during usage make sure to check for null;
         return   GetDeviceInfo.Get(_httpContext.HttpContext?.Request.Headers["User-Agent"].ToString());
        }
    }
    }


