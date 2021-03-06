﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Plugins;
using Plugins.Mail;
using Plugins.Redis.Cache;
using Plugins.Youtube;
using Realist.Api.Controllers;
using Realist.Data;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.Repo;
using Realist.Data.ViewModels;
using Xunit;

namespace RealistTest
{
    public class PostControllerTest
    {
    

   
        [Fact]
        public async Task CreatePost_Pass()
        {
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.mp4";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var postMockOject =new Mock<IPost>();
            postMockOject.Setup(r => r.Post(It.IsAny<Post>())).Returns(Task.CompletedTask);
            var mailSeviceObject = new Mock<IMailService>();
            var loggerObject = new Mock<ILogger<PostController>>();
            var userObject = new Mock<IUser>();
            userObject.Setup(r => r.GetCurrentUser()).Returns(Guid.NewGuid().ToString);
            var photoUploadObject = new  Mock<IPhoto>();
            photoUploadObject.Setup(r => r.UploadImageDb(It.IsAny<Photo>())).ReturnsAsync(Guid.NewGuid());
            var youTubeObject = new Mock<IYoutube>();
            youTubeObject.Setup(r => r.UploadVideo(It.IsAny<UploadViewModel>(), fileMock.Object))
                .ReturnsAsync(GetUpload());
            var videoObject = new Mock<IVideo>();
            videoObject.Setup(r => r.Post(It.IsAny<Videos>())).ReturnsAsync(Guid.NewGuid);
            var mapper = new Mock<IMapper>();
            var photoAccessorObject =new Mock<IPhotoAccessor>();
            photoAccessorObject.Setup(r => r.AddPhoto(fileMock.Object)).Returns(Getupload());
            var redisObject = new Mock<IRedis>();
            var postController = new PostController(postMockOject.Object,mailSeviceObject.Object,loggerObject.Object,userObject.Object,photoUploadObject.Object,youTubeObject.Object,mapper.Object,photoAccessorObject.Object,videoObject.Object,redisObject.Object);
          var result = await postController.CreatePost(It.IsAny<PostModel>());
          Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public void GetAllTest()
        {
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.mp4";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var postMockOject = new Mock<IPost>();
            postMockOject.Setup(r => r.GetAll(It.IsAny<PaginationModel>())).Returns(ValueFunction);
            var mailSeviceObject = new Mock<IMailService>();
            var loggerObject = new Mock<ILogger<PostController>>();
            var userObject = new Mock<IUser>();
            var photoUploadObject = new Mock<IPhoto>();
            var youTubeObject = new Mock<IYoutube>();
            var videoObject = new Mock<IVideo>();
            var mapper = new Mock<IMapper>();
            var photoAccessorObject = new Mock<IPhotoAccessor>();
            var redisObject = new Mock<IRedis>();
            var postController = new PostController(postMockOject.Object, mailSeviceObject.Object, loggerObject.Object, userObject.Object, photoUploadObject.Object, youTubeObject.Object, mapper.Object, photoAccessorObject.Object, videoObject.Object,redisObject.Object);
            var result = postController.GetAll(It.IsAny<PaginationModel>());
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task PostAUser()
        {

            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.mp4";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var postMockOject = new Mock<IPost>();
            postMockOject.Setup(r => r.Get("jhgfdsfghjkljhgf")).ReturnsAsync(GetApost);
            var mailSeviceObject = new Mock<IMailService>();
            var loggerObject = new Mock<ILogger<PostController>>();
            var userObject = new Mock<IUser>();
            var photoUploadObject = new Mock<IPhoto>();
            var youTubeObject = new Mock<IYoutube>();
            var videoObject = new Mock<IVideo>();
            var mapper = new Mock<IMapper>();
            var redisObject = new Mock<IRedis>();
            var photoAccessorObject = new Mock<IPhotoAccessor>();
            var postController = new PostController(postMockOject.Object, mailSeviceObject.Object, loggerObject.Object, userObject.Object, photoUploadObject.Object, youTubeObject.Object, mapper.Object, photoAccessorObject.Object, videoObject.Object,redisObject.Object);
            var result = await postController.Get(It.IsAny<GetPostModel>());
            Assert.IsType<ObjectResult>(result);
        }

       
        private Post GetApost()
        {
            return new Post {};
        }

        private PagedList<Post> ValueFunction()
        {
           return  new PagedList<Post>(It.IsAny<List<Post>>(),2,2,2 );
        }


        private PhotoUpLoadResult Getupload()
        {
         
            return new PhotoUpLoadResult
            {
                Url = "ghsJKSLAskdjhbzsjKALS:Cdk",
                PublicId ="sahjkdl;skdjsaklsd"
            };
        }

        private UploadVideoResult GetUpload()
        {
            return new UploadVideoResult
            {
                    VideoId = "ahjkL:skjdhsk"
            };
        }
    }

}
