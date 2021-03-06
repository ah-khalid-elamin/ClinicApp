// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.15.0

using Bot.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Bot.Controllers
{
    // This ASP Controller is created to handle a request. Dependency Injection will provide the Adapter and IBot
    // implementation at runtime. Multiple different IBot implementations running at different endpoints can be
    // achieved by specifying a more specific type for the bot constructor argument.
    [Route("api/messages")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter Adapter;
        private readonly IBot Bot;
        private readonly ILogger<BotController> _logger;

        public BotController(IBotFrameworkHttpAdapter adapter, IBot bot, ILogger<BotController> logger)
        {
            Adapter = adapter;
            Bot = bot;
            _logger = logger;
        }
        [HttpPost, HttpGet]
        public async Task PostAsync()
        {
            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            var token = HttpContext.Request.Headers.Authorization;
            URIHelpers.Token = token;

            _logger.LogInformation($"access-token: {token}");
            await Adapter.ProcessAsync(Request, Response, Bot);
        }

    }
}
