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
        private readonly IConversationReferenceService _conversationReferenceService;

        public CardController(ICardService cardService, IEmailService emailService,
            IUserService userService, IConversationReferenceService conversationReferenceService)
        {
            _cardService = cardService;
            _emailService = emailService;
            _userService = userService;
        }
    [Authorize]
        [HttpPost("send-card")]
        public async Task<ActionResult> SendCardAsync()
        {
            try
            {
                var userid = Request.HttpContext.User.Claims.ToList()
                .FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                await _cardService.SendCardByUserADId(userid);
            }
            catch (Exception e)
            {
                await Task.FromResult(e);
            }
            return Ok("Card has been sent to the user.");

        }
        [Authorize(Roles = "Admin")]
        [HttpPost("save-user")]
        public async Task<ActionResult> SaveAndSendEmail([FromBody] User user)
        {
            try
            {
                var userid = Request.HttpContext.User.Claims.ToList()
                .FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                await _cardService.SaveAndSendEmail(userid, user);
            }
            catch (Exception e)
            {
                await Task.FromResult(e);
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
        public async Task<ActionResult> GetUser()
        {
            try
            {
                var userId = Request.HttpContext.User.Claims.ToList()
                .FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                var user = _userService.GetUser(userId);
                return Ok(user);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }        
        }
        [HttpPost("send-email")]
        public async Task SendEmail([FromBody] Message message)
        {
            await _emailService.SendEmail(message.To, message.Subject,message.Body);
        }

    }
}
