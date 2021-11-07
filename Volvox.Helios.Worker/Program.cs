using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Volvox.Helios.Core.Bot;
using Volvox.Helios.Core.Bot.Utilities;
using Volvox.Helios.Core.Modules.Command;
using Volvox.Helios.Core.Modules.Command.Commands;
using Volvox.Helios.Core.Modules.Command.Framework;
using Volvox.Helios.Core.Modules.Common;

namespace Volvox.Helios.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Settings
                    services.AddSingleton<IDiscordSettings, DiscordSettings>();

                    // Bot
                    services.AddSingleton<DiscordSocketClient>();
                    services.AddSingleton<IBot, Bot>();

                    // Hosted services
                    services.AddHostedService<Worker>();

                    // Commands
                    services.AddSingleton<IModule, CommandManager>();
                    services.AddSingleton<ICommand, AboutCommand>();

                    // All Modules
                    services.AddSingleton<IList<IModule>>(s => s.GetServices<IModule>().ToList());
                    services.AddSingleton<IList<ICommand>>(s => s.GetServices<ICommand>().ToList());
                });
    }
}
