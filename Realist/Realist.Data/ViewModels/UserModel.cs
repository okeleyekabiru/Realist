using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Realist.Api.ViewModels
{
    public class UserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public  IFormFile Photo { get; set; }
    }
}
