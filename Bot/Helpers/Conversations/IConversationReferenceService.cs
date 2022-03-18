using Common.Models;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Helpers.Conversations
{
    public interface IConversationReferenceService
    {
        Task<List<ConversationReference>> GetAllConversationReferences();
        Task<ConversationReference> GetConversationReferenceByUser(string userId);
        Task<ConversationReference> GetConversationReferenceByADUserId(string AadObjectId);
        Task<ConversationReferenceEntity> GetConversationReference(string conversationId);
        Task<ConversationReferenceEntity> GetConversationReferenceByEmail(string email);
        Task AddOrUpdateConversationReference(ConversationReference reference, TeamsChannelAccount member);
        Task RemoveConversationReference(ConversationReference reference, TeamsChannelAccount member);
    }
}
