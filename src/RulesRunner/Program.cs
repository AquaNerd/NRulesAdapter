using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NRules.Extensibility;
using System.IO;
using static RulesRunner.cqrs.PingCommand;

namespace RulesRunner {
    class Program {
        static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false);
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) => {
                    logging.AddConfiguration(hostingContext.Configuration);
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureServices(ConfigureServices);

        private static void ConfigureServices(IServiceCollection services) {
            services.AddScoped<IDependencyResolver, HostDependencyResolver>();
            services.AddSingleton<IRulesAdapter, RulesAdapter>();
            services.AddMediatR(typeof(PingHandler));
            services.AddHostedService<Worker>();
        }
    }
}