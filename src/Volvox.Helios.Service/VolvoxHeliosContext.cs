using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volvox.Helios.Core.Modules.StreamAnnouncer;
using Volvox.Helios.Domain.Module;
using Volvox.Helios.Domain.Module.ChatTracker;
using Volvox.Helios.Domain.ModuleSettings;

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

        public DbSet<StreamAnnouncerMessage> StreamAnnouncerMessages { get; set;  }

        #endregion

        public DbSet<PollSettings> PollSettings { get; set; }

        #region ChatTracker

        public DbSet<ChatTrackerSettings> ChatTrackerSettings { get; set; }

        public DbSet<Message> Messages { get; set; }

        #endregion

        #region Reminder
        public DbSet<RemembotSettings> ReminderSettings { get; set; }
        public DbSet<RecurringReminderMessage> RecurringReminderMessages { get; set; }
        #endregion

        #region Dad Module
        public DbSet<DadModuleSettings> DadModuleSettings { get; set; }
        #endregion

        #region CleanChat

        public DbSet<CleanChatSettings> CleanChatSettings { get; set; }

        #endregion

        public DbSet<WhiteListedRole> WhiteListedRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SetupForReminderSchema(modelBuilder);
            SetupForDad(modelBuilder);
        }

        private void SetupForDad(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DadModuleSettings>()
                .Property(x => x.DadResponseCooldownMinutes)
                .HasDefaultValue(15);
        }

        private void SetupForReminderSchema(ModelBuilder modelBuilder)
        {
            var reminderModel = modelBuilder.Entity<RemembotSettings>();
            var recurringReminderModel = modelBuilder.Entity<RecurringReminderMessage>();

            modelBuilder.HasSequence<long>("OrderNumbers")
                .StartsAt(1)
                .IncrementsBy(1);

            reminderModel.HasMany(x => x.RecurringReminders);

            recurringReminderModel.HasKey(x => x.Id);

            recurringReminderModel.Property(x => x.Id)
                .HasDefaultValueSql("nextval('\"OrderNumbers\"')");

            recurringReminderModel
                .HasIndex(x => x.GuildId);

            recurringReminderModel.Property(x => x.Enabled)
                .IsRequired();

            recurringReminderModel.Property(x => x.ChannelId)
                .IsRequired();

            recurringReminderModel.Property(x => x.Message)
                .IsRequired();

            recurringReminderModel.Property(x => x.CronExpression)
                .HasMaxLength(255)
                .IsRequired();

            recurringReminderModel.Property(x => x.Fault)
                .HasDefaultValue(RecurringReminderMessage.FaultType.None)
                .IsRequired();

            modelBuilder.Entity<Message>()
                .HasIndex(m => m.GuildId);
        }
    }
}