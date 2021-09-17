using Volvox.Helios.Domain.Common;

namespace Volvox.Helios.Domain.ModuleSettings.StreamerModule
{
    public class StreamAnnouncerMessage : Entity
    {
        public ulong UserId { get; set; }

        public ulong MessageId { get; set; }

        public ulong ChannelId { get; set; }

        public ulong GuildId { get; set; }

        public virtual StreamerSettings StreamerSettings { get; set; }
    }
}