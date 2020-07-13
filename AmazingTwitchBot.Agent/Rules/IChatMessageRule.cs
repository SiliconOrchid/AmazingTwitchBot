using TwitchLib.Client.Events;

namespace AmazingTwitchBot.Agent.Rules
{
    public interface IChatMessageRule
    {
        bool IsTextMatched(string chatMessage);
        string ReturnedMessage(OnMessageReceivedArgs e);
    }
}