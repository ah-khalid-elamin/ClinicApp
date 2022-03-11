using Bot.Helpers.Conversations;
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
    [Route("api/notifications")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;

        public NotifyController(INotificationsService notificationService)
        {
            _notificationsService = notificationService;   
        }
        [HttpPost("notifyUser")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationMessage notificationMessage)
        {
            await _notificationsService.SendNotification(notificationMessage);
            return Ok("User has been notified.");
        }
        [HttpPost("bodcast")]
        public async Task<IActionResult> SendBodcast([FromBody] NotificationMessage notificationMessage)
        {
            await _notificationsService.SendBodcast(notificationMessage);
            return Ok("All Users has been notified.");
        }

    }
}
