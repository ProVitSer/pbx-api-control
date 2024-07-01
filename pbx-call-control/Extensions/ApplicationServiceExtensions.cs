using Microsoft.Extensions.Options;
using PbxApiControl.Interface;
using PbxApiControl.Services.Pbx;
using Microsoft.AspNetCore.Mvc.Razor;

namespace PbxApiControl.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IApiHostSettings>(configuration.GetSection("ApiHostSettings"));
            services.AddScoped<IExtensionService, ExtensionService>();
            services.AddScoped<IRingGroupService, RingGroupService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<ICallService, CallService>();
            services.AddSingleton<IPbxEventListenerService>(provider =>
            {
                var apiHostSettings = provider.GetRequiredService<IOptions<IApiHostSettings>>().Value;
                
                var logger = provider.GetRequiredService<ILogger<PbxEventListenerService>>();
                
                var httpClient = provider.GetRequiredService<HttpClient>();
                
                return PbxEventListenerService.GetInstance(logger, apiHostSettings, httpClient);
            });


            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            
            
        }

    }
}