using Common.Contexts;
using Common.Models;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Helpers.Conversations
{
    public class ConversationReferenceService : BaseTablesHelper<ConversationReferenceEntity>, IConversationReferenceService
    {

        public ConversationReferenceService(IConfiguration configuration) : base(configuration, "ConversationReference", "ConversationReferences")
        {

        }

        public async Task AddOrUpdateConversationReference(ConversationReference reference, TeamsChannelAccount member)
        {
            var entity = ConvertConversationReferanceForDB(reference, member);
            await CreateOrUpdateAsync(entity);
        }

        private ConversationReferenceEntity ConvertConversationReferanceForDB(ConversationReference reference, TeamsChannelAccount currentMember)
        {

            return new ConversationReferenceEntity
            {
                UPN = currentMember.UserPrincipalName,
                Name = currentMember.Name,
                AadObjectId = currentMember.AadObjectId,
                UserId = currentMember.Id,
                ActivityId = reference.ActivityId,
                BotId = reference.Bot.Id,
                ChannelId = reference.ChannelId,
                ConversationId = reference.Conversation.Id,
                Locale = reference.Locale,
                RowKey = currentMember.UserPrincipalName,
                ServiceUrl = reference.ServiceUrl,
                PartitionKey = "ConversationReferences"
            };
        }

        public async Task<ConversationReferenceEntity> GetConversationReference(string conversationId)
        {
            return await GetAsync(conversationId);
        }

        public List<ConversationReferenceEntity> GetAllConversationReferences()
        {
            throw new System.NotImplementedException();
        }

        public  Task RemoveConversationReference(ConversationReference reference, TeamsChannelAccount member)
        {
            var entity = ConvertConversationReferanceForDB(reference, member);
            return DeleteAsync(entity);
        }
    }
}
