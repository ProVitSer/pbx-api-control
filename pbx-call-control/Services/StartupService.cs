using PbxApiControl.Interface;

namespace PbxApiControl.Services
{
    public class StartupService : IHostedService
    {
        private readonly ILogger<StartupService> _logger;
        private readonly IPbxEventListenerService _pbxEventListenerService;

        public StartupService(ILogger<StartupService> logger, IPbxEventListenerService pbxEventListenerService)
        {
            _logger = logger;
            _pbxEventListenerService = pbxEventListenerService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartupService is starting.");

            _pbxEventListenerService.OnStartListenEvent();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartupService is stopping.");
            
            return Task.CompletedTask;
        }
        
    }
}

