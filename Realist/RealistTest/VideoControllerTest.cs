using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Plugins.Youtube;
using Realist.Api.Controllers;
using Realist.Data.Model;
using Xunit;

namespace RealistTest
{
  public  class VideoControllerTest
    {
      

        [Fact]
        public async Task VideoUpload_Test()
        {
            var youtubeMockObject = new Mock<IYoutube>();
            youtubeMockObject.Setup(r => r.UploadVideo(It.IsAny<UploadViewModel>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(GetVideoObject());
            var videoController = new VideoController(youtubeMockObject.Object);
            //Arrange
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


            var result = await videoController.Upload(fileMock.Object);
            Assert.IsType<OkObjectResult>(result);


        }

        private UploadVideoResult GetVideoObject()
        {
            return new UploadVideoResult
            {
                VideoId = "fkjdjxk"
            };
        }
    }
}
