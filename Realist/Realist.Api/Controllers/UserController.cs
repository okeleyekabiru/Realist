using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Plugins;
using Plugins.JwtHandler;
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
        private readonly ILogger<UserController> _logger;

        public UserController(IUser userContext, IPhotoAccessor photoAccessor, IPhoto photoContext,
            ILogger<UserController> logger)
        {
            _userContext = userContext;
            _photoAccessor = photoAccessor;
            _photoContext = photoContext;
            _logger = logger;
        }

        [HttpPost("register")]

        public async Task<ActionResult> Register([FromForm] UserModel user)
        {
          
            JwtModel model;
            try
            {
                user.Email = user.Email.ToLower();
                var verify = await _userContext.EmailExists(user.Email);
                if (verify) return BadRequest(new {Email = "Email Already exist"});
                var photo = _photoAccessor.AddPhoto(user.Photo);

                var users = new User
                {
                    Email = user.Email,
                    Password = user.Password,
                    UserName = user.Email,

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
                model = await _userContext.RegisterUser(users);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message);
                throw;
            }

            if (model.Error != null)
            {
                return BadRequest(new {Error = model.Error});
            }

            return Ok(model);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(SigninModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            try
            {
                var returnModel = await _userContext.Login(model);
                if (returnModel.Error != null) return BadRequest(new {Error = "invalid email or  password"});

                return Ok(returnModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);

            }



            return StatusCode(500, "Internal Server Error");
        }
    }

}