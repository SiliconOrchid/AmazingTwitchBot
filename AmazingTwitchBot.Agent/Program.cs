using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


using AmazingTwitchBot.Agent.Models.Configuration;
using System.Collections.Generic;
using AmazingTwitchBot.Agent.Rules;
using AmazingTwitchBot.Agent.Rules.ChatCommands;
using System.Reflection;
using System.Linq;

namespace AmazingTwitchBot.Agent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();


            IServiceCollection services = new ServiceCollection();
            services.Configure<TwitchConfiguration>(configuration.GetSection(nameof(TwitchConfiguration)));
            services.Configure<ChatConfiguration>(configuration.GetSection(nameof(ChatConfiguration)));
            services.AddSingleton<TwitchChatBot>();

            services.Scan(scan => scan.FromAssemblyOf<IChatMessageRule>()
                .AddClasses(classes => classes.AssignableTo<IChatMessageRule>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime()            
            );


            var serviceProvider = services.BuildServiceProvider();

            TwitchChatBot twitchChatBot = serviceProvider.GetService<TwitchChatBot>();

            twitchChatBot.Connect();
            Console.ReadLine(); 

            twitchChatBot.Disconnect();

        }
    }
}


