using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Realist.Data.Model
{
  public  class User:IdentityUser
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string UserLocation { get; set; }
        public string UserIpHost { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public bool IsUserValid { get; set; }
        public ICollection<Photo>  Photo { get; set; }
        public ICollection<Videos> Videos { get; set; }
        public ICollection<Post> Posts { get; set; }
        public  ICollection<Comment> Comments { get; set; }
        public  ICollection<UserInfo> UserInfos { get; set; }

      
    }
  
}
