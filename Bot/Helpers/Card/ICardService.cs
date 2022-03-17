using System.Threading.Tasks;

namespace Bot.Helpers.Card
{
    public interface ICardService
    {
        public Task sendCardAsync(string UserId);
    }
}
