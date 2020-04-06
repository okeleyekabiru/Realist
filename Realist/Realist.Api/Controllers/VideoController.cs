using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plugins.Youtube;

namespace Realist.Api.Controllers
{
    [Route("api/video/upload")]
    [ApiController]
    public class VideoController : ControllerBase
    {

        private readonly IYoutube _youTubePlugin;

        public VideoController(IYoutube youTubePlugin)
        {
            _youTubePlugin = youTubePlugin;
        }
        [HttpPost("youtube")]
        public async Task<ActionResult> Upload([FromForm] IFormFile video)
        {
            UploadViewModel upload = new UploadViewModel();
            upload.Description = video.Name;
            upload.Type = video.ContentType;
            upload.CategoryId = String.Empty;
            upload.Title = video.FileName;
            upload.VideoTags = new string[]{"tag1","tag2"};
            upload.Private = false;
           var videoUpload= await _youTubePlugin.UploadVideo(upload, video);
           return Ok(videoUpload);

        }
    }
}