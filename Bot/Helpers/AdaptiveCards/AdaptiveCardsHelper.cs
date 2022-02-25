using Microsoft.Bot.Schema;

namespace Bot.Helpers.AdaptiveCards
{
    public class AdaptiveCardsHelper
    {
        public static Attachment GetDoctorCard()
        {
            var card = new Attachment();
            return card;
        }
        public static Attachment GetAppointmentCard()
        {
            var card = new Attachment();
            return card;
        }
        public static Attachment GetPatientCard()
        {
            var card = new Attachment();
            return card;
        }
    }
}
