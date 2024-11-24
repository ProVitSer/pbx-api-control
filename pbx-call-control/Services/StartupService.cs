namespace PbxApiControl.Services
{
    public class StartupService : IHostedService
    {
        private readonly ILogger<StartupService> _logger;

        public StartupService(ILogger<StartupService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartupService is starting.");
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartupService is stopping.");
            
            return Task.CompletedTask;
        }
        
    }
}

