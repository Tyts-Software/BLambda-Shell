using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tyts.BLambda.Shell.Modules.Demo.Notifier;

internal class TimerService : IDisposable
{
    private int elapsedCount;
    private readonly static TimeSpan heartbeatTickRate = TimeSpan.FromSeconds(5);
    private readonly ILogger<TimerService> logger;
    private readonly Lazy<NotifierService> notifier;
    private PeriodicTimer? timer;

    public TimerService(Lazy<NotifierService> notifier,
        ILogger<TimerService> logger)
    {
        this.notifier = notifier;
        this.logger = logger;
    }

    public async Task Start()
    {
        if (timer is null)
        {
            timer = new(heartbeatTickRate);
            logger.LogInformation("Started");

            using (timer)
            {
                while (await timer.WaitForNextTickAsync())
                {
                    elapsedCount += 1;
                    await notifier.Value.Update("elapsedCount", elapsedCount);
                    logger.LogInformation($"elapsedCount: {elapsedCount}");
                }
            }
        }
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}