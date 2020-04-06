using System;
using System.Collections.Generic;
using System.Text;
using Realist.Data.Model;

namespace Plugins.JwtHandler
{
   public interface IJwtSecurity
   {
       JwtModel CreateToken(User user);


       string ReadToken(string token);
       string CreateTokenForEmail(User user);
    }
}
