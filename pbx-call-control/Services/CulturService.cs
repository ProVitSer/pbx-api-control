using System.Globalization;
using Microsoft.AspNetCore.Localization;


namespace PbxApiControl.Services
{
    public class CulturService
    {
    
        public static void SetCulture(WebApplicationBuilder builder)
        {
            var cultureInfo = new CultureInfo("en-US");

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(cultureInfo);
                options.SupportedCultures = new[] { cultureInfo };
                options.SupportedUICultures = new[] { cultureInfo };
            });
        }
    }
}

