using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CatFact
{
    class Program
    {
        public readonly static string CatFactUrl = "https://catfact.ninja";
        public readonly static string CatFactFilePath = "CatFact.txt";
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<IApp>().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IApp, App>();
                    services.AddSingleton<IFileConnector, FileConnector>();
                    services.AddSingleton<IApiConnector, ApiConnector>();
                });
        }
    }
}
