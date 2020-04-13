using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Plugins.Mail;
using Realist.Api.SignalR;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.ViewModels;

namespace Realist.Api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IComment _commemtContext;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IUser _usercontext;
        private readonly ILogger<CommentController> _logger;
        private readonly IHubContext<CommentHub> _hub;

        public CommentController(IComment commemtContext,IMapper mapper,IMailService mailService,IUser usercontext,ILogger<CommentController> logger,IHubContext<CommentHub> hub)
        {
            _commemtContext = commemtContext;
            _mapper = mapper;
            _mailService = mailService;
            _usercontext = usercontext;
            _logger = logger;
            _hub = hub;
        }
        [HttpPost]
        public async Task<ActionResult> Post(CommentsModel comment)
        {
            try
            {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }
                
            var userId = _usercontext.GetCurrentUser();
            var comments = new Comment{
             Body = comment.Body,
             DatePosted = DateTime.Now,
             PostId = Guid.Parse(comment.PostId)

            };
          await  _commemtContext.Add(comments);
          if (!await  _commemtContext.SaveChanges())
          {
              return StatusCode(StatusCodes.Status100Continue, new {Error = "an error occured while processing "});
          }

         

          }
          catch (Exception e)
          {
              _logger.LogError(e.InnerException?.ToString()??e.Message);
              _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message,"error");
              return StatusCode(statusCode: StatusCodes.Status500InternalServerError, "Internal server error");
            }

           

          return Ok(new {Success = "comment Sucessfully posted"});
        }

        public async Task<ActionResult<IEnumerable<CommentsModel>>> Get(IdModel commentsModel)
        {
            IEnumerable<CommentsViewModel> newModel;
          
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.ValidationState);
                }
             var model = await _commemtContext.GetAllPostComment(commentsModel.PostId);
               newModel = _mapper.Map< IEnumerable<Comment>, IEnumerable<CommentsViewModel>>(model);
              await _hub.Clients.All.SendAsync("comment",newModel);
                if (model == null) return NoContent();


            }
            catch (Exception e)
            {

                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message, "error");
                return StatusCode(statusCode: StatusCodes.Status500InternalServerError, "Internal server error");
            }

            return Ok(new {Comment ="Comments loaded successfully"});
        }
        [HttpPatch]
        public async Task<ActionResult> PutComment(CommentsModel comment)
        {
            

            try{
                var model = await _commemtContext.GetComment(comment.CommentId);
                model.Body = comment.Body;
                var result =  await _commemtContext.Update(model);
                if(!result.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError,result.Error);
            }
            catch(Exception e){

              _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message, "error");
                return StatusCode(statusCode: StatusCodes.Status500InternalServerError, "Internal server error");
            }
            return Ok(new {Success = true});
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(IdModel id)
        {
            try
            {
                var model = await _commemtContext.GetComment(id);
                var result = await _commemtContext.Delete(model);
                if (!result.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message, "error");
                return StatusCode(statusCode: StatusCodes.Status500InternalServerError, "Internal server error");
            }
            return Ok(new { Success = true });
        }
    }
}