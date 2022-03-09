// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.15.0

using Bot.Helpers.Conversations;
using Bot.Helpers.RequestResolver;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
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
        private readonly ConcurrentDictionary<string, ConversationReference> ConversationReferences;
        public Bot(IRequestResolver requestResolver, ConcurrentDictionary<string, 
            ConversationReference> conconversationReferences,
            IConversationReferenceService conversationReferenceService)
        {
            this.RequestResovler = requestResolver;
            this.ConversationReferences = conconversationReferences;
            this.ConversationReferenceService = conversationReferenceService;
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var currentUser = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id);

            List<Attachment> attachments = await RequestResovler.Resolve(currentUser, turnContext.Activity.Text);
            
            await turnContext.SendActivityAsync(MessageFactory.Carousel(attachments), cancellationToken);

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

            if (activity.Action.Equals("add"))
                await ConversationReferenceService.AddOrUpdateConversationReference(botConRef, currentMember);
            else if (activity.Action.Equals("remove"))
                await ConversationReferenceService.AddOrUpdateConversationReference(botConRef, currentMember);

        }
    }
}
