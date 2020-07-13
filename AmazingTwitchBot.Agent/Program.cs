using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


using AmazingTwitchBot.Agent.Models.Configuration;


namespace AmazingTwitchBot.Agent
{
    public class Program
    {
        // link to inspiration TwitchBot https://github.com/kasuken/SonequaBot (super straightforward/small example)

        ////credentials (suppressed for privacy)
        //private static string login_name = "<LOGIN_NAME>";
        //private static string token = Environment.GetEnvironmentVariable("Token");  //Token should be stored in a safe place
        //private static List<string> channels_to_join = new List<string>(new string[] { "<CHANNEL_1>", "<CHANNEL_2>" });

        //main function
        public static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                //.AddCommandLine(args)
                .Build();

            IServiceCollection services = new ServiceCollection();
            services.Configure<TwitchConfiguration>(configuration.GetSection(nameof(TwitchConfiguration)));
            services.AddSingleton<TwitchChatBot>();

            var serviceProvider = services.BuildServiceProvider();



            // https://stackoverflow.com/questions/31863981/how-to-resolve-instance-inside-configureservices-in-asp-net-core
            //Foo foo = serviceProvider.GetService<Foo>();
            //ConfigFoo configfoo = serviceProvider.GetService<IOptions<ConfigFoo>>().Value;

            //TwitchConfiguration twitchConfiguration = serviceProvider.GetService<IOptions<TwitchConfiguration>>().Value;

            TwitchChatBot twitchChatBot = serviceProvider.GetService<TwitchChatBot>();

            twitchChatBot.Connect();
            Console.ReadLine();

            twitchChatBot.Disconnect();

        }
    }
}

//    //Testing writing to line
//    Console.WriteLine("Hello World!");

//    //New up a List of TwitchChatBot objects
//    List<TwitchChatBot> chatBots = new List<TwitchChatBot>();

//    //add each channel to the list
//    for (int i = 0; i < channels_to_join.Count; i++)
//    {
//        chatBots.Add(new TwitchChatBot(login_name, token, channels_to_join[i]));
//    }

//    //for each chatBot...
//    for (int i = 0; i < chatBots.Count; i++)
//    {
//        //this chatBot
//        TwitchChatBot chatBot = chatBots[i];

//        //Connect to Twitch IRC
//        chatBot.Connect();

//        //Start Pinger
//        Pinger pinger = new Pinger(chatBot);
//        pinger.Start();
//    }

//    //Read message until we quit
//    while (true)
//    {
//        //for each chatBot...
//        for (int i = 0; i < chatBots.Count; i++)
//        {
//            //this chatbot
//            TwitchChatBot chatBot = chatBots[i];

//            //if we get disconnected, reconnect
//            if (!chatBot.Client.Connected)
//            {
//                chatBot.Connect();
//            }
//            //else we're connected
//            else
//            {
//                //get the message that just came through
//                string msg = chatBot.ReadMessage();

//                //did we receive a message?
//                if (msg != "" && msg != null)
//                {
//                    //write the raw message to the console
//                    Console.WriteLine(msg);

//                    //response string
//                    string toRespond = "";

//                    //If PING respond with PONG
//                    if (msg.Length >= 4 && msg.Substring(0, 4) == "PING")
//                        chatBot.SendPong();

//                    //Trim the message to just the chat message piece
//                    string msgTrimmed = trimMessage(msg);

//                    //Handling commands
//                    if (msgTrimmed.Length >= 6 && msgTrimmed.Substring(0, 6) == "!8ball")
//                        toRespond = chatBot.Command_MagicEightBall();
//                    else if (msgTrimmed == "!age")
//                        toRespond = chatBot.Command_Age();
//                    else if (msgTrimmed == "!discord")
//                        toRespond = chatBot.Command_Discord();

//                    //Write response to console and send message to chat
//                    Console.WriteLine(toRespond);

//                    //Send the message to chat
//                    chatBot.SendMessage(toRespond);
//                }

//            }
//        }

//    }

//}

//#region Helper methods
///// <summary>
///// Trims an IRC message from chat to just the message that was sent in the chat
///// </summary>
///// <param name="message"></param>
///// <returns>string</returns>
//public static string trimMessage(string message)
//{
//    int indexOfSecondColon = getNthIndex(message, ':', 2);
//    var result = message.Substring(indexOfSecondColon + 1);
//    return result;
//}

///// <summary>
///// Gets the second colon, which is the seperator before the chat message
///// </summary>
///// <param name="s"></param>
///// <param name="t"></param>
///// <param name="n"></param>
///// <returns>string</returns>
//public static int getNthIndex(string s, char t, int n)
//{
//    int count = 0;
//    for (int i = 0; i < s.Length; i++)
//    {
//        if (s[i] == t)
//        {
//            count++;
//            if (count == n)
//            {
//                return i;
//            }
//        }
//    }
//    return -1;
//}
//#endregion

//        }
//    }
//}
