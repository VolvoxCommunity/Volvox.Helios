using Microsoft.EntityFrameworkCore;
using Volvox.Helios.Domain.ModuleSettings.StreamerModule;

namespace Volvox.Helios.Service
{
    public class VolvoxHeliosContext : DbContext
    {
        public VolvoxHeliosContext(DbContextOptions<VolvoxHeliosContext> options)
            : base(options)
        {
        }

        #region Streamer

        public DbSet<StreamerSettings> StreamerSettings { get; set; }

        public DbSet<StreamerChannelSettings> StreamerChannelSettings { get; set; }

        public DbSet<StreamAnnouncerMessage> StreamAnnouncerMessages { get; set; }

        #endregion
    }
}