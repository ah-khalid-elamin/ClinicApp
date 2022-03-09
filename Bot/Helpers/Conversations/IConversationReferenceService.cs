using Common.Models;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Helpers.Conversations
{
    public interface IConversationReferenceService
    {
        List<ConversationReferenceEntity> GetAllConversationReferences();
        Task<ConversationReferenceEntity> GetConversationReference(string conversationId);
        Task AddOrUpdateConversationReference(ConversationReference reference, TeamsChannelAccount member);
        Task RemoveConversationReference(ConversationReference reference, TeamsChannelAccount member);
    }
}
