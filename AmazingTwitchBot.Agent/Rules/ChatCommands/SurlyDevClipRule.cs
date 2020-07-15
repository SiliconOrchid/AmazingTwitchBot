using System;

namespace AmazingTwitchBot.Agent.Rules.ChatCommands
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
