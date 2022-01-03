using Microsoft.Extensions.Configuration;

namespace Volvox.Helios.Core.Bot.Utilities
{
    public interface IDiscordSettings
    {
        string Token { get; }

        string ClientId { get; }

        IConfiguration Config { get; }
    }
}
