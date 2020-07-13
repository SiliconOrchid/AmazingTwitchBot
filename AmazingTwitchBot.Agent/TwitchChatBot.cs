using AmazingTwitchBot.Agent.Models.Configuration;
using AmazingTwitchBot.Agent.Rules;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace AmazingTwitchBot.Agent
{

    public class TwitchChatBot
    {
        private readonly List<IChatMessageRule> _listChatMessageRules;


        private readonly ConnectionCredentials _connectionCredentials; 

        private readonly TwitchConfiguration _twitchConfiguration;


        TwitchClient client;
        readonly TwitchAPI twitchAPI = new TwitchAPI();

        string[] BotUsers = new string[] { "SO_Bot", "streamelements" };

        List<string> UsersOnline = new List<string>();

        public TwitchChatBot(IOptions<TwitchConfiguration> twitchConfiguration)
        {
            _twitchConfiguration = twitchConfiguration.Value;
            _connectionCredentials = new ConnectionCredentials(_twitchConfiguration.BotUsername,_twitchConfiguration.BotToken);

            _listChatMessageRules = new List<IChatMessageRule>();
            _listChatMessageRules.Add(new HelloRule() );
            _listChatMessageRules.Add(new ProjectRule());
            _listChatMessageRules.Add(new RustySpoonsRule());
            _listChatMessageRules.Add(new SurlyYouCantBeSeriousRule());
            _listChatMessageRules.Add(new SurlyDevClipRule());
            _listChatMessageRules.Add(new DestructoPupRule());
            

        }

        internal void Connect()
        {
            Console.WriteLine("Connecting...");

            twitchAPI.Settings.ClientId = _twitchConfiguration.ClientId;

            InizializeBot();
        }

        private void InizializeBot()
        {
            client = new TwitchClient();

            client.OnLog += Client_OnLog;
            client.OnConnectionError += Client_OnConnectionError;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnUserTimedout += Client_OnUserTimedout;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnUserJoined += Client_OnUserJoined;
            client.OnUserLeft += Client_OnUserLeft;

            client.Initialize(_connectionCredentials, _twitchConfiguration.ChannelName);
            client.Connect();

            client.OnConnected += Client_OnConnected;
        }

        private void Client_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            client.SendMessage(_twitchConfiguration.ChannelName, $"Hi to everyone. I am AmazingTwitchBot and I am alive. Again. Somehow.");
        }

        private void Client_OnNewSubscriber(object sender, TwitchLib.Client.Events.OnNewSubscriberArgs e)
        {
            client.SendMessage(_twitchConfiguration.ChannelName, $"Thank you for the subscription {e.Subscriber.DisplayName}!!! I really appreciate it!");
        }

        private void Client_OnUserTimedout(object sender, TwitchLib.Client.Events.OnUserTimedoutArgs e)
        {
            client.SendMessage(_twitchConfiguration.ChannelName, $"User {e.UserTimeout.Username} timed out.");
        }

        private void Client_OnWhisperReceived(object sender, TwitchLib.Client.Events.OnWhisperReceivedArgs e)
        {
            //client.SendWhisper(e.WhisperMessage.Username, $"your said: { e.WhisperMessage.Message}");
        }

        private void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            IChatMessageRule rule = _listChatMessageRules.FirstOrDefault(rule => rule.IsTextMatched(e.ChatMessage.Message));

            if(!(rule is null))
            {
                string returnedMessage = rule.ReturnedMessage(e);
                client.SendMessage(_twitchConfiguration.ChannelName, returnedMessage);
            }





            //if (e.ChatMessage.Message.StartsWith("hi", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(_twitchConfiguration.ChannelName, $"Hey there { e.ChatMessage.DisplayName }.");

            //}
            //else 
            if (e.ChatMessage.Message.StartsWith("!uptime", StringComparison.InvariantCultureIgnoreCase))
            {
                var upTime = GetUpTime().Result;

                client.SendMessage(_twitchConfiguration.ChannelName, upTime?.ToString() ?? "Offline");
            }
            //else if (e.ChatMessage.Message.StartsWith("!project", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(_twitchConfiguration.ChannelName, $"I'm working on {"revealing as many credentials to the internet as possible"}.");
            //}
            //else if (e.ChatMessage.Message.StartsWith("!rustyspoons", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(_twitchConfiguration.ChannelName, $"Surly likes rusty spoons");
            //}

            //else if (e.ChatMessage.Message.StartsWith("!surlyyoucantbeserious", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(_twitchConfiguration.ChannelName, $"Did you REALLY just clip that! !?!?!??");
            //}

            //else if (e.ChatMessage.Message.Contains("@surlydev", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(_twitchConfiguration.ChannelName, $"@SurlyDev did you clip that?!");
            //}

            //else if (e.ChatMessage.Message.Contains("!destructopup", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(_twitchConfiguration.ChannelName, $"Stop bloody destroying things!");
            //}


            //else if (e.ChatMessage.Message.StartsWith("!instagram", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(TwitchInfo.ChannelName, $"Follow me on Instagram: {TwitchInfo.Instagram}");
            //}
            //else if (e.ChatMessage.Message.StartsWith("!twitter", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(TwitchInfo.ChannelName, $"Follow me on Twitter: {TwitchInfo.Twitter}");
            //}
            //else if (e.ChatMessage.Message.StartsWith("!blog", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(TwitchInfo.ChannelName, $"My blog: {TwitchInfo.Blog}");
            //}
            //else if (e.ChatMessage.Message.StartsWith("!playlist", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(TwitchInfo.ChannelName, $"Playlist for my live on Twitch: {TwitchInfo.Playlist}");
            //}
            //else if (e.ChatMessage.Message.StartsWith("!discord", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    client.SendMessage(TwitchInfo.ChannelName, $"Vieni sul mio canale Discord: {TwitchInfo.Discord}");
            //}
        }

        private async Task<TimeSpan?> GetUpTime()
        {
            var userId = await GetUserId(_twitchConfiguration.ChannelName);

            return await twitchAPI.V5.Streams.GetUptimeAsync(userId);
        }

        async Task<string> GetUserId(string username)
        {
            var userList = await twitchAPI.V5.Users.GetUserByNameAsync(username);

            return userList.Matches[0].Id;
        }

        private void Client_OnConnectionError(object sender, TwitchLib.Client.Events.OnConnectionErrorArgs e)
        {
            Console.WriteLine(e.Error.Message);
        }

        private void Client_OnLog(object sender, TwitchLib.Client.Events.OnLogArgs e)
        {
            Console.WriteLine(e.Data);
        }



        void Client_OnUserJoined(object sender, TwitchLib.Client.Events.OnUserJoinedArgs e)
        {
            if (BotUsers.Contains(e.Username)) return;

            try
            {
                //client.SendMessage(TwitchInfo.ChannelName, $"Welcome on my channel, { e.Username }.");

                UsersOnline.Add(e.Username);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void Client_OnUserLeft(object sender, TwitchLib.Client.Events.OnUserLeftArgs e)
        {
            UsersOnline.Remove(e.Username);
        }

        internal void Disconnect()
        {
            Console.WriteLine("Disconnecting...");
        }

    }
}


///FOR US TO PLAY WITH NEXT WEEK:
//private void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
//{
//    if (e.ChatMessage.Message.StartsWith("hi", StringComparison.InvariantCultureIgnoreCase))
//    {
//        client.SendMessage(_twitchConfiguration.ChannelName, $"Hey there { e.ChatMessage.DisplayName }.");
//    }
//    else if (e.ChatMessage.Message.StartsWith("!uptime", StringComparison.InvariantCultureIgnoreCase))
//    {
//        var upTime = GetUpTime().Result;

//        client.SendMessage(_twitchConfiguration.ChannelName, upTime?.ToString() ?? "Offline");
//    }
//    else if (e.ChatMessage.Message.StartsWith("!project", StringComparison.InvariantCultureIgnoreCase))
//    {
//        client.SendMessage(_twitchConfiguration.ChannelName, $"I'm working on {"revealing as many credentials to the internet as possible"}.");
//    }
//    else if (e.ChatMessage.Message.StartsWith("!rustyspoons", StringComparison.InvariantCultureIgnoreCase))
//    {
//        client.SendMessage(_twitchConfiguration.ChannelName, $"Surly likes rusty spoons");
//    }

//    else if (e.ChatMessage.Message.StartsWith("!surlyyoucantbeserious", StringComparison.InvariantCultureIgnoreCase))
//    {
//        client.SendMessage(_twitchConfiguration.ChannelName, $"Did you REALLY just clip that! !?!?!??");
//    }

//    else if (e.ChatMessage.Message.Contains("@surlydev", StringComparison.InvariantCultureIgnoreCase))
//    {
//        client.SendMessage(_twitchConfiguration.ChannelName, $"@SurlyDev did you clip that?!");
//    }

//    else if (e.ChatMessage.Message.Contains("!destructopup", StringComparison.InvariantCultureIgnoreCase))
//    {
//        client.SendMessage(_twitchConfiguration.ChannelName, $"Stop bloody destroying things!");
//    }


//    //else if (e.ChatMessage.Message.StartsWith("!instagram", StringComparison.InvariantCultureIgnoreCase))
//    //{
//    //    client.SendMessage(TwitchInfo.ChannelName, $"Follow me on Instagram: {TwitchInfo.Instagram}");
//    //}
//    //else if (e.ChatMessage.Message.StartsWith("!twitter", StringComparison.InvariantCultureIgnoreCase))
//    //{
//    //    client.SendMessage(TwitchInfo.ChannelName, $"Follow me on Twitter: {TwitchInfo.Twitter}");
//    //}
//    //else if (e.ChatMessage.Message.StartsWith("!blog", StringComparison.InvariantCultureIgnoreCase))
//    //{
//    //    client.SendMessage(TwitchInfo.ChannelName, $"My blog: {TwitchInfo.Blog}");
//    //}
//    //else if (e.ChatMessage.Message.StartsWith("!playlist", StringComparison.InvariantCultureIgnoreCase))
//    //{
//    //    client.SendMessage(TwitchInfo.ChannelName, $"Playlist for my live on Twitch: {TwitchInfo.Playlist}");
//    //}
//    //else if (e.ChatMessage.Message.StartsWith("!discord", StringComparison.InvariantCultureIgnoreCase))
//    //{
//    //    client.SendMessage(TwitchInfo.ChannelName, $"Vieni sul mio canale Discord: {TwitchInfo.Discord}");
//    //}
//}


