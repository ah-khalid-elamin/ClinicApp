using Common.Models;
using System.Threading.Tasks;

namespace Bot.Helpers.Card
{
    public interface ICardService
    {
        public Task SendCardByEmailAsync(string email);
        public Task SendCardByUserADId(string UserADId);
        public Task SaveAndSendEmail(string userADId, User user);
    }
}
