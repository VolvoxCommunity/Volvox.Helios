using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volvox.Helios.Core.Bot;

namespace Volvox.Helios.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBot _bot;

        public Worker(ILogger<Worker> logger, IBot bot)
        {
            _logger = logger;
            _bot = bot;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker is starting bot at: {time}", DateTimeOffset.Now);

            // ReSharper disable once AsyncVoidLambda
            stoppingToken.Register(async () => await _bot.Stop());
            await _bot.Start();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _bot.Stop();

            _logger.LogInformation("Worker has stopped bot at {time}", DateTimeOffset.Now);
        }
    }
}
