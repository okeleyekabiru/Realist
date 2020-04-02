using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugins.JwtHandler;
using Realist.Data.Model;

namespace Realist.Data.Infrastructure
{
  public  interface IUser
  {
      User GetCurrentUser(string userId);
      Task<JwtModel> RegisterUser(User user);
  }
}
