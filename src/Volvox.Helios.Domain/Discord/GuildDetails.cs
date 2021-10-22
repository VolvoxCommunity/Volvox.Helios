using System.Collections.Generic;
using System.Collections.ObjectModel;
using Discord.Rest;

namespace Volvox.Helios.Domain.Discord
{
    public class GuildDetails
    {
        public bool IsBotInGuild { get; set; }

        public int MemberCount { get; set; }

        public IReadOnlyCollection<RestRole> Roles { get; set; } = new Collection<RestRole>();

        public IReadOnlyCollection<RestChannel> Channels { get; set; } = new Collection<RestChannel>();
    }
}