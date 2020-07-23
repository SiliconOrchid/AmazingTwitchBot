using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AmazingTwitchBot.Agent.Models.Configuration;
using AmazingTwitchBot.Agent.Rules;


namespace AmazingTwitchBot.Agent
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                .RunConsoleAsync();
        }
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) => ConfigureAppConfiguration(hostingContext, config, args))
                .ConfigureServices(ConfigureService)
                .ConfigureLogging(ConfigureHosting);
        }

        private static void ConfigureAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config, string[] args)
        {
            var envName = hostingContext.HostingEnvironment.EnvironmentName;
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true);
            config.AddUserSecrets<Program>();
            config.AddEnvironmentVariables();

            if (args != null)
            {
                config.AddCommandLine(args);
            }
        }

        private static void ConfigureService(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<TwitchConfiguration>(hostContext.Configuration.GetSection(nameof(TwitchConfiguration)));
            services.Configure<ChatConfiguration>(hostContext.Configuration.GetSection(nameof(ChatConfiguration)));
            services.AddSingleton<TwitchChatBot>();

            services.Scan(scan => scan.FromAssemblyOf<IChatMessageRule>()
                .AddClasses(classes => classes.AssignableTo<IChatMessageRule>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
                );

            services.AddHostedService<MainService>();
        }

        private static void ConfigureHosting(HostBuilderContext hostingContext, ILoggingBuilder logging)
        {
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Trace);
        }
    }
}

