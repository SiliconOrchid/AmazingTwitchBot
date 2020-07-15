using System;

namespace AmazingTwitchBot.Agent.Rules.ChatCommands
{

    public class RustySpoonsRule : IChatMessageRule
    {
        public bool IsTextMatched(string chatMessage)
        {
            return chatMessage.StartsWith("!rustyspoons", StringComparison.InvariantCultureIgnoreCase);
        }

        public string ReturnedMessage(TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            return $"Surly likes rusty spoons";
        }
    }
}
