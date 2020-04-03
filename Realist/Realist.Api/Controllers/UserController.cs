using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Realist.Api.ViewModels;
using Realist.Data.Infrastructure;
using Realist.Data.Model;

namespace Realist.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userContext;

        public UserController(IUser userContext)
        {
            _userContext = userContext;
        }
        [HttpPost("register")]
        
        public async Task<ActionResult> Register(UserModel user)
        {
            user.Email = user.Email.ToLower();
            var verify = await _userContext.EmailExists(user.Email);
            if (verify) return BadRequest(new {Email = "Email Already exist"});
            var users = new User
            {
                Email = user.Email,
                Password = user.Password,
                UserName =  user.Email
            };
          var model = await  _userContext.RegisterUser(users);
          if (model.Error != null)
          {
              return BadRequest(new {Error = model.Error});
          }

          return Ok(model);
        }
    }
}