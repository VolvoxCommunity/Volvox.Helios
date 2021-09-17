using System.ComponentModel.DataAnnotations;

namespace Volvox.Helios.Domain.ModuleSettings.StreamerModule
{
    public class StreamerChannelSettings
    {
        [Key] public ulong ChannelId { get; set; }

        public ulong GuildId { get; set; }

        public StreamerSettings StreamerSettings { get; set; }

        public bool RemoveMessage { get; set; } = true;
    }
}