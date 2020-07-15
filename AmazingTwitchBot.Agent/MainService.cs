using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AmazingTwitchBot.Agent
{
    public class MainService : IHostedService
    {
        private readonly TwitchChatBot _twitchChatBot;

        public MainService(TwitchChatBot twitchChatBot)
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


