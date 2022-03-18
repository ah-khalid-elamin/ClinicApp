using Bot.Helpers.AdaptiveCards;
using Bot.Helpers.Conversations;
using Bot.Helpers.Mail;
using Common.Models;
using Common.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Helpers.Card
{
    public class CardService : ICardService
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly string _appId;
        private readonly IConversationReferenceService _conversationReferenceService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public CardService(IBotFrameworkHttpAdapter adapter, IConfiguration configuration, 
            IConversationReferenceService conversationReferenceService,
            IEmailService emailService, IUserService userService)
        {
            _adapter = adapter;
            _appId = configuration["MicrosoftAppId"] ?? string.Empty;
            _conversationReferenceService = conversationReferenceService;
            _emailService = emailService;
            _userService = userService;
        }

        public async Task SaveAndSendEmail(string userADId, User user)
        {
            var entity = new User()
            {
                Id = userADId,
                AlternativeEmail = user.AlternativeEmail,
                DepartmentName = user.DepartmentName
            };

            _userService.Save(entity);
            var convRef = await _conversationReferenceService.GetConversationReferenceByADUserId(userADId);
            var userName = convRef.User.Name;    

            string replyMessage = $"Hi {userName} from {entity.DepartmentName}. Thank you for signing up.";
            
            await _emailService.SendEmail(user.AlternativeEmail, "Welcome", replyMessage);
        }

        public async Task SendCardByEmailAsync(string email)
        {
            var conRefEntity = await _conversationReferenceService.GetConversationReferenceByEmail(email);

            var userId =  conRefEntity.AadObjectId;
            var conversationRef = await _conversationReferenceService.GetConversationReferenceByADUserId(userId);
            // get user info from entity or from user by user id
            var user = conversationRef.User;
            // get card 
            var card = AdaptiveCardsHelper.GetSignUpCard(user);

            //send card to the user.
            await ((BotAdapter)_adapter).ContinueConversationAsync(_appId, conversationRef,
                  async (context, token) => await SendCard(context, card, token),
                  default(CancellationToken));
        }

        public async Task SendCardByUserADId(string UserADId)
        {
            var conversationRef = await _conversationReferenceService.GetConversationReferenceByADUserId(UserADId);
            // get user info from entity or from user by user id
            var user = conversationRef.User;
            // get card 
            var card = AdaptiveCardsHelper.GetSignUpCard(user);

            //send card to the user.
            await((BotAdapter)_adapter).ContinueConversationAsync(_appId, conversationRef,
                  async (context, token) => await SendCard(context, card, token),
                  default(CancellationToken));

        }

        private async Task SendCard(ITurnContext turnContext, Attachment card, CancellationToken token)
        {
            await turnContext.SendActivityAsync(MessageFactory.Attachment(card));
        }

    }
}
