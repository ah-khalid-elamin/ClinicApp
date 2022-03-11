using System;
using System.Net.Http;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace NotificationFunction
{
    public class NotificationTrigger
    {
        [FunctionName("NotificationTrigger")]
        public void Run([TimerTrigger("1 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            HttpClient client = new HttpClient();
            client.GetStringAsync("https://clinicbot.azurewebsites.net/api/notify");
        }
    }
}
