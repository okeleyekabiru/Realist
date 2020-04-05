using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Realist.Api.ViewModels;
using Realist.Data.Model;

namespace Realist.Data.Infrastructure
{
   public class CreateProfile:Profile
   {
       public CreateProfile()
       {
           CreateMap<User, UserModel>();
       }
    }
}
