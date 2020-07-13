using System;
using System.Collections.Generic;
using System.Text;

namespace AmazingTwitchBot.Agent.Rules
{


    public class SurlyDevClipRule : IChatMessageRule
    {
        public bool IsTextMatched(string chatMessage)
        {
            return chatMessage.StartsWith("@surlydev ", StringComparison.InvariantCultureIgnoreCase);
        }

        public string ReturnedMessage(TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            return $"@SurlyDev did you clip that?!";
        }
    }
}
