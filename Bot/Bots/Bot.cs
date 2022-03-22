// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.15.0

using Bot.Helpers.AdaptiveCards;
using Bot.Helpers.Card;
using Bot.Helpers.Conversations;
using Bot.Helpers.Mail;
using Bot.Helpers.RequestResolver;
using Common.Models;
using Common.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Bots
{
    public class Bot : ActivityHandler
    {
        private readonly IRequestResolver RequestResovler;
        private readonly IConversationReferenceService ConversationReferenceService;
        private readonly IEmailService EmailService;
        private readonly IUserService UserService;
        private readonly ILogger<Bot> Logger;
        private readonly ConcurrentDictionary<string, ConversationReference> ConversationReferences;
        public Bot(IRequestResolver requestResolver, ConcurrentDictionary<string, 
            ConversationReference> conconversationReferences,
            IConversationReferenceService conversationReferenceService,
            IEmailService emailService, IUserService userService,
            ILogger<Bot> logger)
        {
            RequestResovler = requestResolver;
            ConversationReferences = conconversationReferences;
            ConversationReferenceService = conversationReferenceService;
            EmailService = emailService;
            UserService = userService;
            Logger = logger;
        }
        protected override Task<AdaptiveCardInvokeResponse> OnAdaptiveCardInvokeAsync(ITurnContext<IInvokeActivity> turnContext, AdaptiveCardInvokeValue invokeValue, CancellationToken cancellationToken)
        {
            return base.OnAdaptiveCardInvokeAsync(turnContext, invokeValue, cancellationToken);
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var currentUser = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id);
          
            if (turnContext.Activity.Value != null) // action submit
            {
                var formData = turnContext.Activity.Value.ToString();

                var payload = JsonConvert.DeserializeObject<SignUpCardPayload>(formData);


                string replyMessage = $"Hi {payload.User.Name} from {payload.Department}. Thank you for signing up.";

                //saving user
                var user = new User()
                {
                    AlternativeEmail = payload.AlternativeEmail,
                    DepartmentName = payload.Department,
                    Id = payload.User.AadObjectId
                };
                UserService.Save(user);


                //sending email
                await EmailService.SendEmail(payload.AlternativeEmail, "Welcoming a user", replyMessage);

                


            } else if (turnContext.Activity.Text != null) //message
            {
                List<Attachment> attachments = await RequestResovler.Resolve(currentUser, turnContext.Activity.Text);
                await turnContext.SendActivityAsync(MessageFactory.Carousel(attachments), cancellationToken);
            }



        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            ConversationReference botConRef = turnContext.Activity.GetConversationReference();
            var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
            await ConversationReferenceService.AddOrUpdateConversationReference(botConRef, currentMember);
        }
        protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            ConversationReference botConRef = turnContext.Activity.GetConversationReference();
            var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
            await ConversationReferenceService.AddOrUpdateConversationReference(botConRef, currentMember);
        }
        protected override async Task OnInstallationUpdateActivityAsync(ITurnContext<IInstallationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;
            ConversationReference botConRef = turnContext.Activity.GetConversationReference();
            var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);

            //save the conversation reference
            await ConversationReferenceService.AddOrUpdateConversationReference(botConRef, currentMember);
            
            //sending welcome card to the user.
            await turnContext.SendActivityAsync(MessageFactory.Attachment(AdaptiveCardsHelper.GetWelcomeCard(currentMember)), 
                cancellationToken);



        }
        protected override async Task OnInstallationUpdateRemoveAsync(ITurnContext<IInstallationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Calling OnInstallationUpdateRemoveAsync");
            ConversationReference botConRef = turnContext.Activity.GetConversationReference();
            var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
            await ConversationReferenceService.RemoveConversationReference(botConRef, currentMember);
        }
    }
}
