using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Plugins.Mail;
using Plugins.Youtube;
using Realist.Data.Infrastructure;

namespace Realist.Api.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPost _postContext;
        private readonly IMailService _mailService;
        private readonly ILogger<PostController> _logger;
        private readonly IUser _userContext;
        private readonly IPhoto _photoUpload;
        private readonly IYoutube _youtubeuploader;

        public PostController(IPost postContext,IMailService mailService,ILogger<PostController> logger,IUser userContext,IPhoto photoUpload,IYoutube youtubeuploader)
        {
            _postContext = postContext;
            _mailService = mailService;
            _logger = logger;
            _userContext = userContext;
            _photoUpload = photoUpload;
            _youtubeuploader = youtubeuploader;
        }
    }
}