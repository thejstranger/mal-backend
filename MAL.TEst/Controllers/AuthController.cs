using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MAL.Common;
using MAL.DTO;
using MAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MAL.TEst.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private IUserService _userService;
        private readonly IConfigProvider _configProvider;
        private readonly IQueueOperator _queueOperator;

        public AuthController(IUserService userService, IMapper mapper, IConfigProvider configProvider, IQueueOperator queueOperator) : base(mapper)
        {
            _userService = userService;
            _configProvider = configProvider;
            _queueOperator = queueOperator;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetAuthToken([FromBody] UserLoginViewModel login)
        {
            try
            {
                var tokenString = _userService.LoginUser(_mapper.Map<UserDto>(login));
 
                if (!String.IsNullOrEmpty(tokenString))
                {
                    return Ok(new { token = tokenString });
                }

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Email")]
        public IActionResult SendEmail([FromBody] SendEmailViewModel details)
        {
            // TODO Validate email
            var emailCsv = details.Email;

            string connectUri = $"activemq:tcp://{_configProvider.GetActiveMqHost()}:{_configProvider.GetActiveMqPort()}";

            var queueConnectedEvent = new AutoResetEvent(false);

            _queueOperator.TryConnect(connectUri, _configProvider.GetActiveMqUser(),
                _configProvider.GetActiveMqPassword(), queueConnectedEvent, 10000, 2000);

            if (queueConnectedEvent.WaitOne(10000))
            {
                var propertyDict = new Dictionary<string, string>
                {
                    {"email_recipients", emailCsv},
                    {"email_subject", "Forgotten Email"}
                };
                _queueOperator.ProduceMessage("Hello there!", propertyDict);

                _queueOperator.Dispose();
                return Ok("Sent");
            }

            _queueOperator.Dispose();
            return StatusCode(500, "Could not connect to ActiveMQ service.");
        }

    }
}
