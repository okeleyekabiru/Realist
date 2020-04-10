using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Plugins.JwtHandler;
using Realist.Api.ViewModels;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.Services;

namespace Realist.Data.Repo
{
  public  class UserRepo:IUser
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly SignInManager<User> _signManager;
        private readonly IJwtSecurity _jwtSecurity;

        public UserRepo(UserManager<User> userManager,DataContext context , IHttpContextAccessor accessor, SignInManager<User> signManager,IJwtSecurity jwtSecurity)
        {
            _userManager = userManager;
            _context = context;
            _accessor = accessor;
            _signManager = signManager;
            _jwtSecurity = jwtSecurity;
        }
        public string GetCurrentUser()
        {
            return _accessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<Tuple<JwtModel,string>> RegisterUser(UserModel user)
        {
            var users = new User
            {
            UserName =  user.Email,
            Email = user.Email
            };
           var result = await _userManager.CreateAsync(users,user.Password);
            if (result.Succeeded)
            {
              var token = _jwtSecurity.CreateToken(users);
               await _signManager.SignInAsync(users, false);
               var emailToken = _jwtSecurity.CreateTokenForEmail(users);
               token.Code = emailToken.Token;
                return new Tuple<JwtModel, string>(token,users.Id);
           }

            return new Tuple<JwtModel, string>(new JwtModel
            {
                Error = result.Errors.ElementAt(0).Description

            },users.Id
                    );
                
          

        }

        public async Task<bool> EmailExists(string email)
        {
            return  await _userManager.FindByEmailAsync(email) != null;
        }
        public async Task<JwtModel> Login(SigninModel user)
        {
            var result = await _signManager.PasswordSignInAsync(user.Email,
                user.Password, user.RememberMe, false);

           if (result.Succeeded)
           {
               var validateUser = await _userManager.FindByEmailAsync(user.Email);

                if (validateUser.EmailConfirmed == false) return new JwtModel
               {
                   Error =  "Email Not verified"
               };

               return _jwtSecurity.CreateToken(validateUser);
           }
           return new JwtModel
           {
               Error = result.IsNotAllowed.ToString()
           };

        }

        public async Task<bool> EmailConfirmation(string code)
        {
            var token = _jwtSecurity.ReadToken(code);
            if (token.ExpiryDate.CompareTo(DateTime.Now) < 0)
            {
                return false;
            }
            var user = await _userManager.FindByIdAsync(token.Token);

            if (user != null)
            {
                user.EmailConfirmed = true;
              await  _userManager.UpdateAsync(user);
                return true;
            }
            return false;
        }
    }
}
