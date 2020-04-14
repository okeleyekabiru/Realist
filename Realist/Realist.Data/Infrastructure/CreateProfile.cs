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
           CreateMap<Post, PostModel>().ReverseMap();
           CreateMap<Post, PostViewModel>().ForMember(o => o.Videos, e => e.MapFrom(s => s.Videos))
               .ForMember(o => o.Photos, e => e.MapFrom(r => r.Photos)).ReverseMap();
           CreateMap<Comment, CommentsViewModel>().ForMember(o => o.Replies, r => r.MapFrom(s => s.Replies))
               .ReverseMap();
           CreateMap<Reply, ReplyModel>().ReverseMap();
           CreateMap<Reply, ReplyViewModel>().ReverseMap();
       }
    }
}
