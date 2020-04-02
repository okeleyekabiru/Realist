using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Realist.Data.Model;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Plugins.JwtHandler
{
  public  class JwtGenrator:IJwtSecurity
    {
        public JwtModel CreateToken(User user)
        {
            var claim = new List<Claim>
            {

                new Claim(JwtRegisteredClaimNames.NameId, user.Id)


            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my super secret key"));
            var credentials =  new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject= new ClaimsIdentity(claims:claim),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
           var token =  tokenHandler.CreateToken(tokenDescriptor);
          var returnToken = tokenHandler.WriteToken(token);

          return new JwtModel
          {
              ExpiryDate = token.ValidTo,
              Token = returnToken
          };
        }
    }
}
