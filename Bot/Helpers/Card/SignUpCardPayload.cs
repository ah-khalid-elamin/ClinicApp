using Microsoft.Bot.Schema;

namespace Bot.Helpers.Card
{
    public class SignUpCardPayload
    {
        public ChannelAccount User { get; set; }
        public string Department { get; set; }
        public string AlternativeEmail { get; set; }
    }
}
