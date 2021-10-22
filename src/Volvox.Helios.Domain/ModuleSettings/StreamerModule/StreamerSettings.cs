using System.Collections.Generic;

namespace Volvox.Helios.Domain.ModuleSettings.StreamerModule
{
    public class StreamerSettings : BaseModuleSettings
    {
        public List<StreamerChannelSettings> ChannelSettings { get; set; }

        public List<StreamAnnouncerMessage> StreamMessages { get; set; }

        public bool StreamerRoleEnabled { get; set; }

        public ulong RoleId { get; set; }

        // public List<WhiteListedRole> WhiteListedRoleIds { get; set; }
    }
}
