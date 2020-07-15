using System;

namespace AmazingTwitchBot.Agent.Rules.ChatCommands
{
    public class DestructoPupRule : IChatMessageRule
    {
        public bool IsTextMatched(string chatMessage)
        {
            return chatMessage.StartsWith("!destructopup", StringComparison.InvariantCultureIgnoreCase);
        }

        public string ReturnedMessage(TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            return $"Stop bloody destroying things!";
        }
    }
}
