﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Discord;
using Discord.WebSocket;
using Volvox.Helios.Core.Bot.Utilities;
using Volvox.Helios.Core.Modules.Command.Framework;
using Volvox.Helios.Core.Modules.Common;
using Volvox.Helios.Core.Utilities;

namespace Volvox.Helios.Core.Modules.Command
{
    /// <summary>
    ///     Manager for Discord message commands.
    /// </summary>
    public class CommandManager : Module
    {
        /// <summary>
        ///     Manager for Discord message commands.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="modules">List of Command modules to be used by the manager.</param>
        /// <param name="settings">Discord settings.</param>
        /// <param name="config">Configuration.</param>
        public CommandManager(ILogger<IModule> logger, IList<ICommand> modules, IDiscordSettings settings,
            IConfiguration config) : base(settings, logger, config)
        {
            Modules = modules;
        }

        private DiscordSocketClient Client { get; set; }

        private IList<ICommand> Modules { get; }

        /// <summary>
        ///     Returns true if the module is enabled for the specified guild and false if not.
        /// </summary>
        /// <param name="guildId">Id if the guild to check.</param>
        /// <returns>True if the module is enabled for the specified guild and false if not.</returns>
        public override Task<bool> IsEnabledForGuild(ulong guildId)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     Initialize the manager by binding to the MessageReceived event.
        /// </summary>
        /// <param name="client">Client to subscribe to.</param>
        public override Task Init(DiscordSocketClient client)
        {
            Client = client;
            Client.MessageReceived += HandleCommand;

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Handle the MessageReceived event.
        /// </summary>
        /// <param name="emitted">Message that was sent.</param>
        private async Task HandleCommand(SocketMessage emitted)
        {
            if (emitted is not SocketUserMessage message || emitted.Channel is IDMChannel ||
                !message.Content.StartsWith("h-", StringComparison.InvariantCulture) || message.Author.IsBot) return;

            var context = new CommandContext(message, Client, "h-");

            foreach (var module in Modules)
                try
                {
                    await module.TryTrigger(context);
                }
                catch (TriggerFailException e)
                {
                    Logger.LogDebug($"Command Manager: Trigger fail exception occurred {e.Message}");
                }
                catch (Exception e)
                {
                    Logger.LogError($"Command Manager: Error occurred {e.Message}");

                    var embed = new EmbedBuilder()
                        .WithColor(EmbedColors.ErrorColor)
                        .WithTitle("Well, this is embarrassing...")
                        .WithDescription(
                            $"Something went ***very*** wrong trying to run that command. \n```\n{e.Message}\n```")
                        .Build();

                    await context.Channel.SendMessageAsync("", false, embed);
                }
        }
    }
}
