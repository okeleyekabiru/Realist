using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realist.Data.Model;
using Realist.Data.ViewModels;

namespace Realist.Data.Infrastructure
{
   public  interface IUserInfo
   {
       Task<ReturnResult> Add(UserInfo userInfo);
       Task<UserInfo> Get(string deviceName, string ipAddress);
   }
}
