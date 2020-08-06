using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using AmazingTwitchBot.Agent.Services;


namespace AmazingTwitchBot.Agent.Services
{
    public class MainService : IHostedService
    {
        private readonly TwitchChatBotService _twitchChatBot;

        public MainService(TwitchChatBotService twitchChatBot)
        {
            _twitchChatBot = twitchChatBot;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _twitchChatBot.Connect();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _twitchChatBot.Disconnect();
            return Task.CompletedTask;
        }
    }
}
