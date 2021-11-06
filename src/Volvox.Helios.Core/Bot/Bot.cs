using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Discord;
using Discord.WebSocket;
using Volvox.Helios.Core.Bot.Connector;
using Volvox.Helios.Core.Bot.Reliability;
using Volvox.Helios.Core.Bot.Utilities;
using Volvox.Helios.Core.Modules.Common;
using Volvox.Helios.Core.Utilities;

namespace Volvox.Helios.Core.Bot
{
    public class Bot : IBot
    {
        private const long AlphaBotsGuildId = 906500033778704454;
        private const long AlphaBotsGuildLogsChannelId = 906500541545349120;

        public DiscordSocketClient Client { get; }
        public IBotConnector Connector { get; }
        public IList<IModule> Modules { get; }
        public ILogger<Bot> Logger { get; }

        /// <summary>
        ///     Discord bot.
        /// </summary>
        /// <param name="modules">List of modules for the bot.</param>
        /// <param name="settings">Settings used to connect to Discord.</param>
        /// <param name="logger">Application logger.</param>
        /// <param name="client"><see cref="DiscordSocketClient"/> for the bot.</param>
        public Bot(IList<IModule> modules, IDiscordSettings settings, ILogger<Bot> logger, DiscordSocketClient client)
        {
            Modules = modules;
            Logger = logger;
            Client = client;

            Client.Log += Log;

            // Log when the bot is disconnected.
            Client.Disconnected += exception =>
            {
                Logger.LogCritical(exception, "Bot has been disconnected!");

                return Task.CompletedTask;
            };

            // Set bot game.
            Client.Ready += () =>
            {
                Task.Run(async () =>
                {
                    for (; ; )
                    {
                        var memberCount = Client.Guilds.Sum(guild => guild.MemberCount);
                        var version = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Version;

                        await Client.SetGameAsync(
                            $"volvox.tech | {Client.Guilds.Count} servers | {memberCount} members | v{version!.Major}.{version.Minor}.{version.Build}");
                        await Task.Delay(TimeSpan.FromMinutes(15));
                    }
                    // ReSharper disable once FunctionNeverReturns
                });

                return Task.CompletedTask;
            };

            // Announce to home when the bot joins a guild.
            Client.JoinedGuild += async guild =>
            {
                var channel = Client.GetGuild(AlphaBotsGuildId)?.GetTextChannel(AlphaBotsGuildLogsChannelId);

                if (channel != null)
                {
                    var builder = new EmbedBuilder
                    {
                        Title = "New Guild",
                        Color = EmbedColors.GuildJoinColor
                    };

                    await channel.SendMessageAsync("", false, CreateGuildEmbed(builder, guild));
                }
            };

            // Announce to home when the bot leaves a guild.
            Client.LeftGuild += async guild =>
            {
                var channel = Client.GetGuild(AlphaBotsGuildId)?.GetTextChannel(AlphaBotsGuildLogsChannelId);

                if (channel != null)
                {
                    var builder = new EmbedBuilder
                    {
                        Title = "Left Guild",
                        Color = EmbedColors.ErrorColor
                    };

                    await channel.SendMessageAsync("", false, CreateGuildEmbed(builder, guild));
                }
            };

            // Add reliability service.
            _ = new ReliabilityService(Client, logger);

            Connector = new BotConnector(settings, Client);
        }

        /// <summary>
        /// Create and build an embed for guild joins/leaves
        /// </summary>
        /// <param name="builder">The EmbedBuilder to be built</param>
        /// <param name="guild">The guild that the bot has joined/left</param>
        /// <returns>The built Embed object</returns>
        private static Embed CreateGuildEmbed(EmbedBuilder builder, SocketGuild guild)
        {
            // ReSharper disable RedundantArgumentDefaultValue
            builder.AddField("Name", guild.Name, false);
            builder.AddField("Owner", $"{guild.Owner.Username} ({guild.OwnerId})", true);
            builder.AddField("User Count", guild.Users.Count(u => !u.IsBot), false);
            builder.AddField("Bot Count", guild.Users.Count(u => u.IsBot), true);
            builder.ThumbnailUrl = guild.IconUrl;

            return builder.Build();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Initialize all of modules available to the bot.
        /// </summary>
        public async Task InitModules()
        {
            foreach (var module in Modules)
            {
                Logger.LogInformation($"Initializing {module.GetType().Name}");
                await module.Init(Client);
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Start the bot.
        /// </summary>
        public async Task Start()
        {
            await InitModules();

            await Connector.Connect();

            await Task.Delay(Timeout.Infinite);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Stop the bot.
        /// </summary>
        public async Task Stop()
        {
            Logger.LogInformation("Disconnecting bot naturally");

            await Connector.Disconnect();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Get a list of the guilds the bot is in.
        /// </summary>
        /// <returns>List of the guilds the bot is in.</returns>
        public IReadOnlyCollection<SocketGuild> GetGuilds()
        {
            return Client.Guilds;
        }

        /// <summary>
        ///     Get the specified guild.
        /// </summary>
        /// <returns>SocketGuild object.</returns>
        public SocketGuild GetGuild(ulong guildId)
        {
            return GetGuilds().FirstOrDefault(g => g.Id == guildId)!;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Returns true if the specified guild is in the bot and false otherwise.
        /// </summary>
        /// <returns>Returns true if the specified guild is in the bot and false otherwise.</returns>
        public bool IsBotInGuild(ulong guildId)
        {
            return GetGuilds().Any(g => g.Id == guildId);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Get the bots role in the hierarchy of the specified guild.
        /// </summary>
        /// <param name="guildId">Id of the guild to get the hierarchy from.</param>
        /// <returns>Bots role position.</returns>
        public int GetBotRoleHierarchy(ulong guildId)
        {
            var hierarchy = GetGuilds()?.FirstOrDefault(g => g.Id == guildId)?.CurrentUser.Hierarchy;

            return hierarchy ?? 0;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Log an event.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public Task Log(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    Logger.LogCritical(message.Message);
                    break;
                case LogSeverity.Error:
                    Logger.LogError(message.Message);
                    break;
                case LogSeverity.Warning:
                    Logger.LogWarning(message.Message);
                    break;
                case LogSeverity.Info:
                    Logger.LogInformation(message.Message);
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Logger.LogTrace(message.Message);
                    break;
                default:
                    Logger.LogInformation(message.Message);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
