using Microsoft.Extensions.Configuration;

namespace Volvox.Helios.Core.Bot.Utilities
{
    public class DiscordSettings : IDiscordSettings
    {
        public DiscordSettings(IConfiguration config)
        {
            Token = config["BotSecrets:Token"];
            ClientId = config["BotSecrets:ClientID"];
            Config = config;
        }

        public string Token { get; }

        public string ClientId { get; }

        public IConfiguration Config { get; }
    }
}
