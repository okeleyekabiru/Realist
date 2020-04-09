using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Plugins;
using Plugins.Mail;
using Plugins.Redis.Cache;
using Plugins.Youtube;
using Realist.Data;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.ViewModels;

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
        private readonly IMapper _mapper;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IVideo _videoContext;
        private readonly IRedis _redis;
      


        public PostController(IPost postContext, IMailService mailService, ILogger<PostController> logger,
            IUser userContext, IPhoto photoUpload, IYoutube youtubeuploader, IMapper mapper,
            IPhotoAccessor photoAccessor, IVideo videoContext, IRedis redis)
        {
            _postContext = postContext;
            _mailService = mailService;
            _logger = logger;
            _userContext = userContext;
            _photoUpload = photoUpload;
            _youtubeuploader = youtubeuploader;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
            _videoContext = videoContext;
            _redis = redis;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("create")]
        public async Task<ActionResult> CreatePost([FromForm] PostModel post)
        {
            var userId = _userContext.GetCurrentUser();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new {Error = "Body can not be empty"});
                }

                var model = _mapper.Map<PostModel, Post>(post);
                model.UserId = userId;
                model.DatePosted = DateTime.Now;
                await _postContext.Post(model);
                if (post.Photo != null)
                {
                    var photoUpload = _photoAccessor.AddPhoto(post.Photo);
                    var photo = new Photo
                    {
                        PostId = model.Id,
                        PublicId = photoUpload.PublicId,
                        UploadTime = DateTime.Now,
                        Url = photoUpload.Url,
                        UserId = userId
                    };


                    await _photoUpload.UploadImageDb(photo);
                }

                if (post.Video != null)
                {
                    var video = new Videos();

                    UploadViewModel upload = new UploadViewModel();
                    upload.Description = post.Video.Name;
                    upload.Type = post.Video.ContentType;
                    upload.CategoryId = String.Empty;
                    upload.Title = post.Video.FileName;
                    upload.VideoTags = new string[] {"tag1", "tag2"};
                    upload.Private = false;
                    var videoUpload = await _youtubeuploader.UploadVideo(upload, post.Video);

                    if (!string.IsNullOrEmpty(videoUpload.VideoId))
                    {
                        video.DateUploaded = DateTime.Now;
                        video.UserId = userId;
                        video.PublicId = videoUpload.VideoId;
                        video.PostId = model.Id;
                        await _videoContext.Post(video);
                    }
                }

                if (!await _videoContext.SaveChanges())
                {
                    return BadRequest(new {Error = "Error Uploading to database"});
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty,
                    _mailService.ErrorMessage(e.InnerException?.ToString() ?? e.Message), "error");
                return StatusCode(500, "Internal Server Error");
            }

            return Ok(new {Post = "Successfully upload"});
        }

        [HttpGet("all")]
        public ActionResult GetAll([FromQuery] PaginationModel page)
        {
            PagedList<Post> posts;
            try
            {
                posts = _postContext.GetAll(page);
                if (posts.Count < 1) return NoContent();

                var metadata = new
                {
                    posts.TotalCount,
                    posts.PageSize,
                    posts.CurrentPage,
                    posts.TotalPages,
                    posts.HasNext,
                    posts.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty,
                    _mailService.ErrorMessage(e.InnerException?.ToString() ?? e.Message), "error");
                return StatusCode(500, "Internal server error");
            }

            return Ok(posts);
        }

        [HttpGet("id")]
        public async Task<ActionResult> Get(GetPostModel id)
        {
            Post model;
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id.Id))
                {
                    return BadRequest();
                }

                var redis = await _redis.GetRedis<Post>(id.Id);
                if (redis == null)
                {
                    model = await _redis.SetRedis(await _postContext.Get(id.Id), id.Id);
                    if (model == null) return NotFound();
                }
                else
                {
                    return Ok(redis);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message, "error");
                return StatusCode(500, "Internal Server Error");
            }

            return Ok(model);
        }
    }
}