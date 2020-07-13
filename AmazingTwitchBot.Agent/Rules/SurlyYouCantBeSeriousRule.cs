using System;
using System.Collections.Generic;
using System.Text;

namespace AmazingTwitchBot.Agent.Rules
{
    public class SurlyYouCantBeSeriousRule : IChatMessageRule
    {
        public bool IsTextMatched(string chatMessage)
        {
            return chatMessage.StartsWith("!surlyyoucantbeserious ", StringComparison.InvariantCultureIgnoreCase);
        }

        public string ReturnedMessage(TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            return $"Did you REALLY just clip that! !?!?!??";
        }
    }
}
