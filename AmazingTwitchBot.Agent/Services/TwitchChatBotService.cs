using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;

using AmazingTwitchBot.Agent.Models.Configuration;
using AmazingTwitchBot.Agent.Rules;


namespace AmazingTwitchBot.Agent.Services
{

    public class TwitchChatBotService
    {
        private readonly IEnumerable<IChatMessageRule> _listChatMessageRules;
        private readonly ILogger<TwitchChatBotService> _logger;
        private readonly ConnectionCredentials _twitchLibConnectionCredentials;
        private readonly TwitchConfiguration _twitchConfiguration;

        private TwitchClient _twitchLibClient = new TwitchClient();
        private TwitchAPI _twitchLibAPI = new TwitchAPI();

        private string[] _BotUsers = new string[] { "SO_Bot", "streamelements" };

        private List<string> _currentUsersOnline = new List<string>();



        public TwitchChatBotService(
            IOptions<TwitchConfiguration> twitchConfiguration,
            IEnumerable<IChatMessageRule> listChatMessageRules,
            ILogger<TwitchChatBotService> logger
            )
        {
            _twitchConfiguration = twitchConfiguration.Value;
            _listChatMessageRules = listChatMessageRules.ToArray();
            _logger = logger;
            _twitchLibConnectionCredentials = new ConnectionCredentials(_twitchConfiguration.BotUsername, _twitchConfiguration.BotToken);
        }



        internal void Connect()
        {
            _twitchLibAPI.Settings.ClientId = _twitchConfiguration.ClientId;
            InizializeBot();
        }

        private void InizializeBot()
        {
            _twitchLibClient.OnLog += Client_OnLog;
            _twitchLibClient.OnConnectionError += Client_OnConnectionError;
            _twitchLibClient.OnMessageReceived += Client_OnMessageReceived;
            _twitchLibClient.OnWhisperReceived += Client_OnWhisperReceived;
            _twitchLibClient.OnUserTimedout += Client_OnUserTimedout;
            _twitchLibClient.OnNewSubscriber += Client_OnNewSubscriber;
            _twitchLibClient.OnUserJoined += Client_OnUserJoined;
            _twitchLibClient.OnUserLeft += Client_OnUserLeft;

            _twitchLibClient.Initialize(_twitchLibConnectionCredentials, _twitchConfiguration.ChannelName);
            _twitchLibClient.Connect();

            _twitchLibClient.OnConnected += Client_OnConnected;
        }

        private void Client_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            _twitchLibClient.SendMessage(_twitchConfiguration.ChannelName, $"Hi to everyone. I am AmazingTwitchBot and I am alive. Again. Somehow.");
        }

        private void Client_OnNewSubscriber(object sender, TwitchLib.Client.Events.OnNewSubscriberArgs e)
        {
            _twitchLibClient.SendMessage(_twitchConfiguration.ChannelName, $"Thank you for the subscription {e.Subscriber.DisplayName}!!! I really appreciate it!");
        }

        private void Client_OnUserTimedout(object sender, TwitchLib.Client.Events.OnUserTimedoutArgs e)
        {
            _twitchLibClient.SendMessage(_twitchConfiguration.ChannelName, $"User {e.UserTimeout.Username} timed out.");
        }

        private void Client_OnWhisperReceived(object sender, TwitchLib.Client.Events.OnWhisperReceivedArgs e)
        {
            //client.SendWhisper(e.WhisperMessage.Username, $"your said: { e.WhisperMessage.Message}");
        }

        private void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            IChatMessageRule chatmessageRule = _listChatMessageRules.FirstOrDefault(rule => rule.IsTextMatched(e.ChatMessage.Message));

            if (!(chatmessageRule is null))
            {
                string messageReturnedFromRule = chatmessageRule.ReturnedMessage(e);
                _twitchLibClient.SendMessage(_twitchConfiguration.ChannelName, messageReturnedFromRule);
            }


            if (e.ChatMessage.Message.StartsWith("!uptime", StringComparison.InvariantCultureIgnoreCase))
            {
                var upTime = GetUpTime().Result;
                _twitchLibClient.SendMessage(_twitchConfiguration.ChannelName, upTime?.ToString() ?? "Offline");
            }

        }

        private async Task<TimeSpan?> GetUpTime()
        {
            var userId = await GetUserId(_twitchConfiguration.ChannelName);
            return await _twitchLibAPI.V5.Streams.GetUptimeAsync(userId);
        }

        async Task<string> GetUserId(string username)
        {
            var userList = await _twitchLibAPI.V5.Users.GetUserByNameAsync(username);

            return userList.Matches[0].Id;
        }

        private void Client_OnConnectionError(object sender, TwitchLib.Client.Events.OnConnectionErrorArgs e)
        {
            _logger.LogError($"ConnectionError: {e.Error.Message}");
        }

        private void Client_OnLog(object sender, TwitchLib.Client.Events.OnLogArgs e)
        {
            _logger.LogInformation($"Client_OnLog: {e.Data}");
        }



        void Client_OnUserJoined(object sender, TwitchLib.Client.Events.OnUserJoinedArgs e)
        {
            if (_BotUsers.Contains(e.Username)) return;

            try
            {
                //client.SendMessage(TwitchInfo.ChannelName, $"Welcome on my channel, { e.Username }.");
                _currentUsersOnline.Add(e.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding user to userlist", ex);
            }
        }

        void Client_OnUserLeft(object sender, TwitchLib.Client.Events.OnUserLeftArgs e)
        {
            _currentUsersOnline.Remove(e.Username);
        }

        internal void Disconnect()
        {
            _logger.LogInformation("Disconnecting...");
        }

    }
}



