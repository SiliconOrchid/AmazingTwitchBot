using System;
using System.Collections.Generic;
using System.Text;

namespace AmazingTwitchBot.Agent.Rules
{
    public class DestructoPupRule : IChatMessageRule
    {
        public bool IsTextMatched(string chatMessage)
        {
            return chatMessage.StartsWith("!destructopup ", StringComparison.InvariantCultureIgnoreCase);
        }

        public string ReturnedMessage(TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            return $"Stop bloody destroying things!";
        }
    }
}
