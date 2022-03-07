using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
               .AddJsonFile("Settings.job")
               .AddEnvironmentVariables()
               .Build();

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(Configuration);

            serviceCollection.AddLogging(builder =>
            {
                builder.ClearProviders();
            });

            Console.WriteLine("Hello.");

        }
    }
}
