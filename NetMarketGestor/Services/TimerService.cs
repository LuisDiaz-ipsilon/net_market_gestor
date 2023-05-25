using Microsoft.Extensions.Logging;

namespace NetMarketGestor.Services
{
    public class TimerService : IHostedService
    {
        private Timer? timer;
        //private ILogger logger;
        private int executionCount = 0;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //logger.LogInformation("WebAPI esta funcionando.");
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public void DoWork(object? state)
        {
            int contador = Interlocked.Increment(ref executionCount);
            //logger.LogInformation("WebAPI esta funcionando, tiempo de ejecucion: {Count:#,0}", contador);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //logger.LogInformation("WebAPI se ha detenido.");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        //public async ValueTask Di

    }
}
