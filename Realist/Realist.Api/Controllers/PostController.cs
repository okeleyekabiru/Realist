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
using Realist.Data.Extensions;
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
        [HttpPost]
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
                var postUpload = await post.UploadPost(userId, _photoUpload, _photoAccessor, _youtubeuploader,
                    _videoContext,
                    _postContext, model);
                if (!postUpload)
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

        [HttpGet]
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

            var newModel = _mapper.Map<List<Post>, List<PostViewModel>>(posts);
            return Ok(newModel);
        }

        [HttpGet("id")]
        public async Task<ActionResult> Get(GetPostModel id)
        {
            Post model;
            PostViewModel newModel;
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id.Id))
                {
                    return BadRequest();
                }

                var redis = await _redis.GetRedis<Post>(id.Id);
                if (redis == null)
                {
                    model = await _redis.SetRedis(await _postContext.GetPost(id.Id), id.Id);
                    if (model == null) return NotFound();
                }
                else
                {
                    newModel
                        = _mapper.Map<Post, PostViewModel>(redis);
                    newModel.CommentCount = await _postContext.GetCommentCount(id.Id);
                    return Ok(newModel);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message, "error");
                return StatusCode(500, "Internal Server Error");
            }

             newModel
                = _mapper.Map<Post, PostViewModel>(model);
            return Ok(newModel);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromForm] PostModel post)
        {
            var userId = _userContext.GetCurrentUser();
            try
            {
                var posts = await _postContext.Get(post.Id);
                post.Body ??= posts.Body;
                var model = _mapper.Map(post, posts);

                var postUpload = await post.UpdatePost(userId, _photoUpload, _photoAccessor, _youtubeuploader,
                    _videoContext,
                    _postContext, model, _mapper);
                if (!postUpload)
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

            return Ok(new {Post = "Successfully updated"});
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(PostModel post)
        {
            ReturnResult result;
            try
            {
                var userId = _userContext.GetCurrentUser();
                if (string.IsNullOrEmpty(post.Id)) return BadRequest(new {Error = "Invalid data"});
                var posts = await _postContext.Get(postId: post.Id);
                result = await posts.Delete(userId, _photoUpload, _photoAccessor, _youtubeuploader, _videoContext,
                    _postContext);


                if (result.Succeeded)
                {
                    return Ok(new {Success = "Entity as been successfully deleted"});
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message, "error");
                return StatusCode(500, "Internal server error");
            }

            return BadRequest(new {Error = result});
        }
    }
}