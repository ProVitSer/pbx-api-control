using PbxApiControl.Interface;
using PbxApiControl.Services.Pbx;
using Microsoft.AspNetCore.Mvc.Razor;

namespace PbxApiControl.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ISqlService, SqlService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            
            
        }

    }
}