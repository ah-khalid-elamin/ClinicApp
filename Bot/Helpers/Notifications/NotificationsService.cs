using Bot.Helpers.Conversations;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Helpers.Notifications
{
    public class NotificationsService : INotificationsService
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly string _appId;
        private readonly IConversationReferenceService _conversationReferenceService;

        public NotificationsService(IBotFrameworkHttpAdapter adapter, IConfiguration configuration,
            IConversationReferenceService conversationReferenceService)
        {
            _adapter = adapter;
            _appId = configuration["MicrosoftAppId"] ?? string.Empty;
            _conversationReferenceService = conversationReferenceService;
        }
        public async Task SendBodcast(NotificationMessage notification)
        {
            var conversationReferences = await _conversationReferenceService.GetAllConversationReferences();
            foreach (var conversationReference in conversationReferences)
            {
                await((BotAdapter)_adapter).ContinueConversationAsync(_appId, conversationReference, 
                    async (context, token) => await SendMessage(context, notification.Message,token),
                    default(CancellationToken));
            }
        }

        public async Task SendNotification(NotificationMessage notification)
        {
            var conversationReference = await _conversationReferenceService.GetConversationReferenceByUser(notification.UserId);
            await ((BotAdapter)_adapter).ContinueConversationAsync(_appId, conversationReference,
                   async (context, token) => await SendMessage(context, notification.Message, token),
                   default(CancellationToken));
        }

        private async Task SendMessage(ITurnContext turnContext, string message, CancellationToken token)
        {
            await turnContext.SendActivityAsync(MessageFactory.Text(message));
        }
    }
}
