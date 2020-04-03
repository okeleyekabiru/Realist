using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plugins;
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
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IPhoto _photoContext;

        public UserController(IUser userContext,IPhotoAccessor photoAccessor,IPhoto photoContext)
        {
            _userContext = userContext;
            _photoAccessor = photoAccessor;
            _photoContext = photoContext;
        }
        [HttpPost("register")]
        
        public async Task<ActionResult> Register([FromForm] UserModel user)
        {
            user.Email = user.Email.ToLower();
            var verify = await _userContext.EmailExists(user.Email);
            if (verify) return BadRequest(new {Email = "Email Already exist"});
            var photo = _photoAccessor.AddPhoto(user.Photo);
        
            var users = new User
            {
                Email = user.Email,
                Password = user.Password,
                UserName =  user.Email,
                
            };
           
            var photoUpload = new Photo
            {
                IsMain = !await _photoContext.FindUserImage(users.Id), 
                PublicId = photo.PublicId,
                Url = photo.Url,
                UploadTime = DateTime.Now,
                UserId = users.Id
                
            };

            await _photoContext.UploadImageDb(photoUpload);
            await _photoContext.SaveChanges();
            var model = await  _userContext.RegisterUser(users);
          if (model.Error != null)
          {
              return BadRequest(new {Error = model.Error});
          }

          return Ok(model);
        }
    }
}