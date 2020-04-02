﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Plugins.JwtHandler;
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
        public User GetCurrentUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<JwtModel> RegisterUser(User user)
        {
           var result = await _userManager.CreateAsync(user,user.Password);
           if (result.Succeeded)
           {
              var token = _jwtSecurity.CreateToken(user);
               await _signManager.SignInAsync(user, false);

               return token;
           }

           return null;

        }
    }
}