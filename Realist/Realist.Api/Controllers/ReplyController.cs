using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugins.Mail;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Realist.Api.SignalR;
using AutoMapper;

namespace Realist.Api.Controllers
{
    [Route("api/reply")]
    [ApiController]
    public class ReplyController : ControllerBase
    {
        private readonly IReply _replyContext;
        private readonly IMailService _mailService;
        private readonly ILogger<ReplyController> _logger;
        private readonly IMapper _mapper;
        private readonly IHubContext<CommentHub> _hub;
        private readonly IComment _commemtContext;

        public ReplyController(IReply replyContext, IMailService mailService, ILogger<ReplyController> logger,
            IUser user, IMapper mapper, IHubContext<CommentHub> hub, IComment commemtContext)
        {
            _commemtContext = commemtContext;
            _hub = hub;
            _replyContext = replyContext;
            _mailService = mailService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> PostReply(ReplyModel reply)
        {
            if (reply.CommentId == null && reply.Body == null)
            {
                return BadRequest(new {ValidationError = "body can not be null"});
            }

            try
            {
                var model = _mapper.Map<ReplyModel, Reply>(reply);
                model.DatePosted =  DateTime.Now;
                var result = await _replyContext.Add(model);
                if (!result.Succeeded)
                    return StatusCode(statusCode: StatusCodes.Status500InternalServerError, result.Error);
                var comments = await _commemtContext.GetAllPostComment(reply.CommentId);
                var newModel = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentsViewModel>>(comments);
                await _hub.Clients.All.SendAsync("comment", newModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message, "error");
                return StatusCode(500, "Internal server error");
            }

            return Ok(new {Success = true});
        }
        [HttpGet]
        public async Task<ActionResult> Get(ReplyModel replyId)
        {
            ReplyViewModel mapModel;
            try
            {
                if (string.IsNullOrEmpty(replyId.ReplyId)) return BadRequest(new {Error ="replyId can not be null"});
                var model = await _replyContext.Get(replyId.ReplyId);
                if (model == null) return NotFound(new {Error = "not reply found"});

                mapModel = _mapper.Map<Reply, ReplyViewModel>(model);
            }
            catch (Exception e)
            {

                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message, "error");
                return StatusCode(500, "Internal server error");
            }

            return Ok(mapModel);
        }

        [HttpPut]
        public async Task<ActionResult> PutReply(ReplyModel reply)
        {

            try
            {
                var getReply = await _replyContext.Get(reply.ReplyId);
                if (getReply == null) return NotFound();
                getReply.Body = reply.Body ?? getReply.Body;
                getReply.Updated = DateTime.Now;
                var result = await _replyContext.Update(getReply);
                if(!result.Succeeded) return StatusCode(500, result.Error);

            }
            catch (Exception e)
            {
                
                    _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message, "error");
                return StatusCode(500, "Internal server error");
            }

            return Ok(new {Success = true});
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(ReplyModel reply)
        {
            if (reply.ReplyId == null) return BadRequest(new {Error = "reply id is required"});
            try
            {
                var replyValue =await _replyContext.Get(reply.ReplyId);
                if (replyValue == null) return NotFound();
                var result = await _replyContext.Delete(replyValue);
                if(!result.Succeeded) return StatusCode(500, result.Error);


            }
            catch (Exception e)
            {

                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                _mailService.SendMail(string.Empty, e.InnerException?.ToString() ?? e.Message, "error");
                return StatusCode(500, "Internal server error");
            }

            return Ok(new {Success = true});

        }
    }
}