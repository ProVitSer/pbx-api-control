using PbxApiControl.Services;
using Microsoft.Extensions.DependencyInjection;

namespace PbxApiControl.Extensions;


public static class ApplicationServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {

        services.AddScoped<IPbxService, PbxService>();

    }

}
