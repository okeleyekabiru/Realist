using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Plugins.JwtHandler;
using Realist.Api.ViewModels;
using Realist.Data.Model;
using Realist.Data.ViewModels;

namespace Realist.Data.Infrastructure
{
   public class CreateProfile:Profile
   {
       public CreateProfile()
       {
           CreateMap<User, UserModel>().ReverseMap();
           CreateMap<JwtModel, UserReturnModel>().ReverseMap();
       }
    }
}
