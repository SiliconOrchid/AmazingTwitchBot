﻿using System;

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
        // link to inspiration TwitchBot https://github.com/kasuken/SonequaBot (super straightforward/small example)

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


            //var messages = Assembly.GetAssembly(typeof(IChatMessageRule))
            //    .GetTypes()
            //    .Where(x => x.Namespace == "AmazingTwitchBot.Agent.Rules.ChatCommands")
            //    .Where(x => x.IsClass)
            //    .Select(x => (IChatMessageRule)Activator.CreateInstance(x))
            //    ;

            //services.AddSingleton(messages);

            // ---- this block could (should) be extracted out into a setup class

            services.Scan(scan => scan.FromAssemblyOf<IChatMessageRule>()
                .AddClasses(classes => classes.AssignableTo<IChatMessageRule>())
                    // We then specify what type we want to register these classes as.
                    // In this case, we want to register the types as all of its implemented interfaces.
                    // So if a type implements 3 interfaces; A, B, C, we'd end up with three separate registrations.
                    .AsImplementedInterfaces()
                    // And lastly, we specify the lifetime of these registrations.
                    .WithSingletonLifetime()            
            );
            
/*
            services.AddSingleton<List<IChatMessageRule>>();
            services.AddSingleton<IChatMessageRule, HelloRule>();
            services.AddSingleton<IChatMessageRule, ProjectRule>();
            services.AddSingleton<IChatMessageRule, RustySpoonsRule>();
            services.AddSingleton<IChatMessageRule, SurlyYouCantBeSeriousRule>();
            services.AddSingleton<IChatMessageRule, SurlyDevClipRule>();
            services.AddSingleton<IChatMessageRule, DestructoPupRule>();
            services.AddSingleton<IChatMessageRule, InstagramRule>();
            services.AddSingleton<IChatMessageRule, TwitterRule>();
            services.AddSingleton<IChatMessageRule, BlogRule>();
            // ------------------------------------------------
*/

            var serviceProvider = services.BuildServiceProvider();





        // https://stackoverflow.com/questions/31863981/how-to-resolve-instance-inside-configureservices-in-asp-net-core
        //Foo foo = serviceProvider.GetService<Foo>();
        //ConfigFoo configfoo = serviceProvider.GetService<IOptions<ConfigFoo>>().Value;

        //TwitchConfiguration twitchConfiguration = serviceProvider.GetService<IOptions<TwitchConfiguration>>().Value;

        TwitchChatBot twitchChatBot = serviceProvider.GetService<TwitchChatBot>();

            twitchChatBot.Connect();
            Console.ReadLine(); // we do this to pause the console app, so it doesn't just run and exit immedietely.

            twitchChatBot.Disconnect();

        }
    }
}


