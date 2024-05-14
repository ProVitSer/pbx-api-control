using PbxApiControl.Services.Pbx;
using PbxApiControl.Interface;

namespace PbxApiControl.Extensions;

public static class ApplicationServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IExtensionService, ExtensionService>();

    }

}