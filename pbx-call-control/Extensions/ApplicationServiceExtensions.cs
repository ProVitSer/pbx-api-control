using PbxApiControl.Services.Pbx;
using PbxApiControl.Interface;
using Microsoft.AspNetCore.Mvc.Razor;

namespace PbxApiControl.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IExtensionService, ExtensionService>();
            services.AddScoped<IRingGroupService, RingGroupService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<ICallService, CallService>();
            services.AddSingleton<IPbxEventListenerService>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<PbxEventListenerService>>();
                return PbxEventListenerService.GetInstance(logger);
            });


            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            
            
        }

    }
}