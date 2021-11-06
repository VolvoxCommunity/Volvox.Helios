using Discord;

namespace Volvox.Helios.Core.Utilities
{
    public static class EmbedColors
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static Color LogoColor = new(102, 204, 51);

        public static Color TwitchColor = new(100, 65, 165);

        public static Color ErrorColor = new(255, 0, 0);

        public static Color GuildJoinColor = new(0, 191, 255);
#pragma warning restore CA2211 // Non-constant fields should not be visible
    }
}
