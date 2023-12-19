using PbxApiControl.Services;
using Microsoft.Extensions.DependencyInjection;
using PbxApiControl.Interface;

namespace PbxApiControl.Extensions;

public static class ApplicationServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {

        services.AddScoped<IPbxService, PbxService>();
        services.AddScoped<IGetActiveConnectionService, GetActiveConnectionsService>();
        services.AddScoped<ICallService, CallService>();


    }

}
