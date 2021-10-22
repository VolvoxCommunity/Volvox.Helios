using System.ComponentModel.DataAnnotations;

namespace Volvox.Helios.Domain.ModuleSettings
{
    public class BaseModuleSettings
    {
        [Key]
        public ulong GuildId { get; set; }

        public bool Enabled { get; set; }
    }
}
