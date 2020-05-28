using System;
using System.Collections.Generic;
using System.Text;
using DeviceDetectorNET;
using DeviceDetectorNET.Cache;

namespace Plugins.DeviceAuthentication
{
    public class GetDeviceInfo
    {
        /// <summary>
        /// this library get device and user related details to perform first level authentication
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns>DeviceInfo</returns>
        public  static  DeviceInfo Get(string userAgent)
        {
            var deviceInfo = new DeviceInfo();
            var client = new ClientModel();
            var bot = new BotModel();
            var os = new OsInfo();
            var device = new DeviceDetector(userAgent);
            
            device.SetCache(new DictionaryCache());
            device.Parse();
            if (device.IsBot())
            {
                // checks if the user is a bot or crawler and retrieve the info
                var botInfo = device.GetBot();
                bot.Success = botInfo.Success;
                bot.Name= botInfo.Matches[0].Name??botInfo.Match?.Name;
                bot.Producer =  botInfo.Matches[0].Producer.Name??botInfo.Match?.Producer.Name;
                bot.ProducerUrl =  botInfo.Matches[0].Producer.Url?? botInfo.Match?.Producer.Url ;
                bot.Url =botInfo.Matches[0].Url ?? botInfo.Match?.Url;

               
            }

            else
            {//if its not a bot get client info
                var clientInfo = device.GetClient();
                client.Name =  clientInfo.Matches[0]?.Name??clientInfo.Match?.Name;
                client.Type =  clientInfo.Matches[0]?.Type??clientInfo.Match?.Type;
                client.Version = clientInfo.Matches[0]?.Version??clientInfo.Match?.Version;
                client.Success = clientInfo.Success;
                    

                // holds information about browser, feed reader, media player, ...
                var osInfo = device.GetOs();
                os.Name = osInfo.Matches[0]?.Name ?? osInfo.Match?.Name;
              os.Version=  osInfo.Match?.Version?? osInfo.Matches[0]?.Version??osInfo.Match?.Version;
             os.PlatForm=   osInfo.Match?.Platform?? osInfo.Matches[0]?.Platform??osInfo.Match?.Platform;
             os.ShortName=   osInfo.Match?.ShortName?? osInfo.Matches[0]?.ShortName??osInfo.Match?.ShortName;
             os.Success=   osInfo.Success;
               client.DeviceName = device.GetDeviceName();
               client.DeviceBrandName = device.GetBrandName();
                client.DeviceModel = device.GetModel();


            }

            client.IsDesktop = device.IsDesktop();
            client.IsMobile = device.IsMobile();
            deviceInfo.Bot = bot;
            deviceInfo.Client = client;
            deviceInfo.OsInfo = os;
            return deviceInfo;

        }
        

    
    }
}
