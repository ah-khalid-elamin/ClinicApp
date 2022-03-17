using Common.Contexts;
using Common.Models;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Bot.Builder.Teams;

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

        public async Task<List<ConversationReference>> GetAllConversationReferences()
        {
            List<ConversationReferenceEntity> entities = await GetAllAsync();
            List<ConversationReference> conversationReferences = new List<ConversationReference>();

            foreach (var entity in entities)
            {
                conversationReferences.Add(GetConversationReferenceFromEntity(entity));
            }

            return conversationReferences;
        }
        private ConversationReference GetConversationReferenceFromEntity(ConversationReferenceEntity entity)
        {
            return new ConversationReference()
            {
                User = new ChannelAccount()
                {
                    Id = entity.UserId,
                    Name = entity.Name,
                    AadObjectId = entity.AadObjectId
                },
               
                Conversation = new ConversationAccount()
                {
                    Id = entity.ConversationId
                },
                ServiceUrl = entity.ServiceUrl,
            };
        }
        public  Task RemoveConversationReference(ConversationReference reference, TeamsChannelAccount member)
        {
            var entity = ConvertConversationReferanceForDB(reference, member);
            return DeleteAsync(entity);
        }

        public async Task<ConversationReference> GetConversationReferenceByUser(string userId)
        {
            List<ConversationReference> conversationReferences = await GetAllConversationReferences();
            return conversationReferences.AsEnumerable()
                        .Where(conversation => conversation?.User?.Id == userId).ToList().FirstOrDefault();
        }
        public async Task<ConversationReference> GetConversationReferenceByADUserId(string AadObjectId)
        {
            var entites = await GetAllAsync();
            var entity =  entites.AsEnumerable().Where(e => e.AadObjectId == AadObjectId)
                .ToList().FirstOrDefault();

            return GetConversationReferenceFromEntity(entity);
          
        }
    }
}
