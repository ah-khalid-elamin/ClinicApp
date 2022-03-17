// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.15.0

using Bot.Helpers.Card;
using Bot.Helpers.Conversations;
using Bot.Helpers.Mail;
using Bot.Helpers.RequestResolver;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
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
        private readonly ConcurrentDictionary<string, ConversationReference> ConversationReferences;
        public Bot(IRequestResolver requestResolver, ConcurrentDictionary<string, 
            ConversationReference> conconversationReferences,
            IConversationReferenceService conversationReferenceService,
            IEmailService emailService)
        {
            RequestResovler = requestResolver;
            ConversationReferences = conconversationReferences;
            ConversationReferenceService = conversationReferenceService;
            EmailService = emailService;
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
                
                await EmailService.SendEmail(payload.AlternativeEmail, "Welcoming a user", replyMessage);

                await turnContext.SendActivityAsync(MessageFactory.Text(replyMessage), cancellationToken);

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

            await turnContext.SendActivityAsync(MessageFactory.Text("Welcome to our Clinic bot!"), cancellationToken);

        }
        protected override async Task OnInstallationUpdateActivityAsync(ITurnContext<IInstallationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;
            ConversationReference botConRef = turnContext.Activity.GetConversationReference();
            var currentMember = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);

            await ConversationReferenceService.AddOrUpdateConversationReference(botConRef, currentMember);
            
            await turnContext.SendActivityAsync(MessageFactory.Text("Welcome to our Clinic bot!"), cancellationToken);


        }
    }
}
