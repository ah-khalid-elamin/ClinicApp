using Bot.Helpers.Notifications;
using System.Threading.Tasks;

namespace Bot.Helpers.Notifications
{
    public interface INotificationsService
    {
        public Task SendNotification(NotificationMessage notification);
        public Task SendBodcast(NotificationMessage notification);


    }
}
