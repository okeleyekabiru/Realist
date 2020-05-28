using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Plugins;
using Plugins.JwtHandler;
using Plugins.Mail;
using Realist.Api.Controllers;
using Realist.Api.ViewModels;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.ViewModels;
using Xunit;

namespace RealistTest
{
    public class UserControllerTest
    {
        public  IFormFile Photo  { get; set; } = new FormFile(Stream.Null, Int64.MaxValue, Int64.MaxValue, String.Empty,String.Empty );
       
        [Fact]
        public async  Task Register_pass()
        {
            var userMockObject = new Mock<IUser>();
            userMockObject.Setup(r => r.RegisterUser(It.IsAny<UserModel>())).ReturnsAsync(GetJwtModel());
            var photoAccessorMockObject = new Mock<IPhotoAccessor>();
            photoAccessorMockObject.Setup(r => r.AddPhoto(Photo)).Returns(GetUpload());
            var photoMockObject = new Mock<IPhoto>();
            photoMockObject.Setup(r => r.UploadImageDb(It.IsAny<Photo>())).ReturnsAsync(Guid.NewGuid());
            var loggerMockObject = new Mock<ILogger<UserController>>();
            var mailServiceObject =  new Mock<IMailService>();
            mailServiceObject.Setup(
                r => r.VerifyEmail("kabiru@gmailcom", "jfdkls;dkfkld;sdskvs;smyhntgbfvdcs78654t3re"));
            var mapper = new Mock<IMapper>();
           
            var userController = new UserController(userMockObject.Object,photoAccessorMockObject.Object,photoMockObject.Object,loggerMockObject.Object,mailServiceObject.Object,mapper.Object);
            var result = await userController.Register(GetUserModel());
             Assert.IsType<OkObjectResult>(result);


        }

        [Fact]
        public async Task Login_Pass()
        {
            var userMockObject = new Mock<IUser>();
            userMockObject.Setup(r => r.Login(It.IsAny<SigninModel>())).Returns(GetJwtModel());
            var photoAccessorMockObject = new Mock<IPhotoAccessor>();
            photoAccessorMockObject.Setup(r => r.AddPhoto(Photo)).Returns(GetUpload());
            var photoMockObject = new Mock<IPhoto>();
            photoMockObject.Setup(r => r.UploadImageDb(It.IsAny<Photo>())).ReturnsAsync(Guid.NewGuid());
            var loggerMockObject = new Mock<ILogger<UserController>>();
            var mailServiceObject = new Mock<IMailService>();
            mailServiceObject.Setup(
                r => r.VerifyEmail("kabiru@gmailcom", "jfdkls;dkfkld;sdskvs;smyhntgbfvdcs78654t3re"));
            var mapper = new Mock<IMapper>();

            var userController = new UserController(userMockObject.Object, photoAccessorMockObject.Object, photoMockObject.Object, loggerMockObject.Object, mailServiceObject.Object, mapper.Object);
            var result = await userController.Login(It.IsAny<SigninModel>());
            Assert.IsType<OkObjectResult>(result);
        }

        private Func<Tuple<JwtModel, string>> GetJwtModel()
        {
          return  new Func<Tuple<JwtModel, string>>(Target);
        }

        private Tuple<JwtModel, string> Target()
        {
              return new Tuple<JwtModel, string>(new JwtModel
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI5Njg5OTBiOC00YTk5LTRjMjMtODkxMi01Yzg5YzE4MzdjMjciLCJuYmYiOjE1ODY0OTgzNjgsImV4cCI6MTU4NzEwMzE2OCwiaWF0IjoxNTg2NDk4MzY4fQ.5t_HLyxl1Dh5kxaff0U1pTWe19CgXKqhUoFWlQ5HvlI",
                ExpiryDate = DateTime.Now
            }, "dshjakl;sskfjdsj");
        }


        private UserModel GetUserModel()
        {
            return new UserModel
            {
                Email = "kabiotobiano@gmail.com",
                Password = "Abiola12345%",
            

            };
        }

        [Fact]
        public async Task ConfirmEmail()
        {

            var userMockObject = new Mock<IUser>();
            userMockObject.Setup(r => r.EmailConfirmation("BHJEFKLCS;CKJGNGBFKDVLCSCDFKNJ")).Returns(Task.FromResult(true));
            var photoAccessorMockObject = new Mock<IPhotoAccessor>();
            photoAccessorMockObject.Setup(r => r.AddPhoto(Photo)).Returns(GetUpload());
            var photoMockObject = new Mock<IPhoto>();
            photoMockObject.Setup(r => r.UploadImageDb(It.IsAny<Photo>())).ReturnsAsync(Guid.NewGuid());
            var loggerMockObject = new Mock<ILogger<UserController>>();
            var mailServiceObject = new Mock<IMailService>();
            mailServiceObject.Setup(
                r => r.VerifyEmail("kabiru@gmailcom", "jfdkls;dkfkld;sdskvs;smyhntgbfvdcs78654t3re"));
            var mapper = new Mock<IMapper>();

            var userController = new UserController(userMockObject.Object, photoAccessorMockObject.Object, photoMockObject.Object, loggerMockObject.Object, mailServiceObject.Object, mapper.Object);
            var result = await userController.Confirmation("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJmOGM4MWZiOS1iZTJkLTQ3ZGEtOWM4My1jYWE4ODE5ZDQzMzUiLCJuYmYiOjE1ODYzNTg1NDgsImV4cCI6MTU4Njk2MzM0OCwiaWF0IjoxNTg2MzU4NTQ4fQ.8P6Z-n7IbTwkh2S91p9H9dthr8aH2jS35fg0Y_c-t6Q");
            Assert.IsType<BadRequestObjectResult>(result);
        }

     


        private PhotoUpLoadResult  GetUpload()
        {
           return  new PhotoUpLoadResult();
        }

       
    }
}
