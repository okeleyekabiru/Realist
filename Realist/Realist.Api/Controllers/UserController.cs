﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog.Targets;
using Plugins;
using Plugins.JwtHandler;
using Plugins.Mail;
using Realist.Api.ViewModels;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.ViewModels;

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
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public UserController(IUser userContext, IPhotoAccessor photoAccessor, IPhoto photoContext,
            ILogger<UserController> logger,IMailService mailService,IMapper mapper)
        {
            _userContext = userContext;
            _photoAccessor = photoAccessor;
            _photoContext = photoContext;
            _logger = logger;
            _mailService = mailService;
            _mapper = mapper;
       
        }

        [HttpPost("register")]

        public async Task<ActionResult> Register([FromBody] UserModel user)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState.ValidationState);
            }
            JwtModel model;
            User users;
            try
            {
                user.Email = user.Email.ToLower();
                var verify = await _userContext.EmailExists(user.Email);
                if (verify) return BadRequest(new {Email = "Email Already exist"});
                users = new User
                {Email = user.Email,
                    UserName = user.UserName,
                    Password = user.Password
                };
                model = await _userContext.RegisterUser(users);
                if (user.Photo != null)
                {
                    var photo = _photoAccessor.AddPhoto(user.Photo);

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
                }

            }
            catch (Exception e)
            {
                var mail = _mailService.ErrorMessage(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail("", mail, "error");
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                throw;

            }

            if (model.Error != null)
            {
                return BadRequest(new { model.Error});
            }
            _mailService.VerifyEmail(user.Email,model.Code);
            var newModel = _mapper.Map<JwtModel, UserReturnModel>(model);
            return Ok(newModel);
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
                var newModel = _mapper.Map<JwtModel, UserReturnModel>(returnModel);
                if (returnModel.Error != null) return BadRequest(returnModel.Error);
                return Ok(newModel);

            }
            catch (Exception e)
            {
              var mail =  _mailService.ErrorMessage(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail("",mail,"error");
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);

            }



            return StatusCode(500, "Internal Server Error");
        }
        [HttpPost("confirmation")]
        public async Task<ActionResult> Confirmation(string token)
        {
            bool model;
            
            try
            {
                
              model =  await _userContext.EmailConfirmation(token);
              if (!model) return BadRequest(new {Error = "invalid user token or token as expired"});
            }
            catch (Exception e)
            {
                
                _logger.LogError(e.InnerException?.ToString()??e.Message);
                _mailService.SendMail(String.Empty, e.InnerException?.ToString() ?? e.Message,"error");
                return StatusCode(500, "Internal Server");
            }
            
            return Ok(new {success="Email Successful Verified"});
        }
    }

}