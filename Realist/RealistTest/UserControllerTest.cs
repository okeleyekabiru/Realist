using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Plugins;
using Plugins.Mail;
using Realist.Api.Controllers;
using Realist.Data.Infrastructure;
using Xunit;

namespace RealistTest
{
    public class UserControllerTest
    {
        public UserControllerTest(IUser userContext, IPhotoAccessor photoAccessor, IPhoto photoContext,
            ILogger<UserController> logger, IMailService mailService, IMapper mapper)
        {
            
        }
        [Fact]
        public void Register_pass()
        {

        }
    }
}
