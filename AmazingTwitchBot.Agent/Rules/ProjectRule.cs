using System;
using System.Collections.Generic;
using System.Text;

namespace AmazingTwitchBot.Agent.Rules
{
    public class ProjectRule : IChatMessageRule
    {
        public bool IsTextMatched(string chatMessage)
        {
            //todo:  evaluate text length, rather than startwith
            return chatMessage.StartsWith("!project ", StringComparison.InvariantCultureIgnoreCase);
        }

        public string ReturnedMessage(TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            return $"I'm working on revealing as many credentials to the internet as possible";
        }
    }

}
