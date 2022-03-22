using Bot.Helpers;
using Bot.Helpers.Card;
using Bot.Helpers.Conversations;
using Bot.Helpers.Mail;
using Bot.Helpers.Notifications;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Controllers
{
    [Route("api/assessment")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly ILogger<CardController> _logger;
        private readonly IConversationReferenceService _conversationReferenceService;

        public CardController(ICardService cardService, IEmailService emailService,
            IUserService userService, IConversationReferenceService conversationReferenceService,
            ILogger<CardController> logger)
        {
            _cardService = cardService;
            _emailService = emailService;
            _userService = userService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("send-card")]
        public async Task<ActionResult> SendCardAsync()
        {
            try
            {
                _logger.LogInformation("Call SendCardAsync()");
                var userid = Request.HttpContext.User.Claims.ToList()
                .FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                await _cardService.SendCardByUserADId(userid);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return Ok("Card has been sent to the user.");

        }
        [Authorize(Roles = "Admin")]
        [HttpPost("save-user")]
        public async Task<ActionResult> SaveAndSendEmail([FromBody] User user)
        {
            try
            {
                _logger.LogInformation($"Call SaveAndSendEmail({user.DepartmentName}, {user.AlternativeEmail})");
                var userid = Request.HttpContext.User.Claims.ToList()
                .FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                await _cardService.SaveAndSendEmail(userid, user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return Ok("User information saved successfully.");

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("get-users")]
        public ActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("get-my-user")]
        public  ActionResult GetUser()
        {

            var token = HttpContext.Request.Headers.Authorization;
            URIHelpers.Token = token;

            _logger.LogInformation($"access-token: {token}");

            User user = new();
            try
            {

                var userId = Request.HttpContext.User.Claims.ToList()
                .FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                _logger.LogInformation($"Call GetUser with oid {userId}.");

                 user = _userService.GetUser(userId);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return Ok(user);
        }
        [HttpPost("send-email")]
        public async Task SendEmail([FromBody] Message message)
        {
            await _emailService.SendEmail(message.To, message.Subject,message.Body);
        }

    }
}
