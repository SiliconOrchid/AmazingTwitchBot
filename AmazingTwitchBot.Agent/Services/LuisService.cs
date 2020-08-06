using System.Net.Http;
using Microsoft.Extensions.Options;
using AmazingTwitchBot.Agent.Models.Configuration;


namespace AmazingTwitchBot.Agent.Services
{
    public class LuisService
    {
        private readonly HttpClient _httpClient;


        private readonly Luisconfiguration _luisconfiguration;

        ///TODO :  
        /// makes a request to the LUIS service, per Twitch chat message.  Send an "utterance"
        /// returns json object containing "the intent" combined with "certainty"
        /// 

        // TODO parse returned JSON, extract "intent" and "certainty".
        // Presumably an object that represents that.


        // create a ruleset handler that returns a chat response (defined in configuration).

        public LuisService(IOptions<Luisconfiguration> luisconfiguration, HttpClient httpClient)
        {
            _luisconfiguration = luisconfiguration.Value;
            _httpClient = httpClient;
        }
    }
}
