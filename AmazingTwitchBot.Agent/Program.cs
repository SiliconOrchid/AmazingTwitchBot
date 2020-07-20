using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


using AmazingTwitchBot.Agent.Models.Configuration;
using AmazingTwitchBot.Agent.Rules;
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


			var messages = Assembly.GetAssembly(typeof(IChatMessageRule))
				.GetTypes()
				.Where(x => x.Namespace == "AmazingTwitchBot.Agent.Rules.ChatCommands")
				.Where(x => x.IsClass)
				.Where(x => !x.IsAbstract);


			foreach (var messageType in messages)
			{
				services.AddSingleton(serviceProvider => (IChatMessageRule)ActivatorUtilities.CreateInstance(serviceProvider, messageType));
			}
			

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


