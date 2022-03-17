using Bot.Helpers.Card;
using Bot.Helpers.Conversations;
using Bot.Helpers.Mail;
using Bot.Helpers.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Controllers
{
    [Route("api/cards")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IEmailService _emailService;

        public CardController(ICardService cardService, IEmailService emailService)
        {
            _cardService = cardService;   
            _emailService = emailService;   
        }
        [HttpPost("send-card/{Id}")]
        public async Task SendCardAsync([FromRoute] string Id)
        {
            await _cardService.sendCardAsync(Id);
        }
        [HttpPost("send-email")]
        public async Task SendEmail([FromBody] Message message)
        {
            await _emailService.SendEmail(message.To, message.Subject,message.Body);
        }

    }
}
