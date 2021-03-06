﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugins.JwtHandler;
using Realist.Api.ViewModels;
using Realist.Data.Model;

namespace Realist.Data.Infrastructure
{
  public  interface IUser
  {
      string GetCurrentUser();
      Task<Tuple<JwtModel,string> >RegisterUser(UserModel user);
     Task< bool >EmailExists(string email);
     Task<JwtModel> Login(SigninModel model);
     Task<bool> EmailConfirmation(string code);





  }
}
