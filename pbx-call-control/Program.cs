using PbxApiControl.Services.Grpc;
using PbxApiControl.Extensions;
using PbxApiControl.Config;
using PbxApiControl.Interceptor;
using PbxApiControl.Services;
using Serilog;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Server.Kestrel.Core;


namespace PbxApiControl
{
    public class Program
    {
        
        public static void Main(string[] args)
        {

            // Configure Serilog
            LoggerService.ConfigureLogger(args);

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();
            builder.Services.AddWindowsService(options =>
            {
                options.ServiceName = "PbxApiControl";
            });
            
            CulturService.SetCulture(builder);

            builder.Services.AddHttpClient();
            
            // Add services to the container.
            builder.Services.AddGrpc(options =>
                {
                    options.Interceptors.Add<ExceptionInterceptor>();
                    options.Interceptors.Add<LoggingInterceptor>();
                })
                .AddJsonTranscoding(); // Add support for JSON transcoding
            
            // Initialize PBX configuration
            PbxApiConfig.InitConfig();
            
            builder.Services.AddGrpcReflection();
            builder.Services.AddApplicationServices();
            builder.Services.AddHostedService<StartupService>();
            if (OperatingSystem.IsWindows())
            {
                builder.Services.AddHostedService<WindowsBackgroundService>();
            }

            // Configure Kestrel server
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(new Uri("http://127.0.0.1:7878").Port, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });

            });
            
            var app = builder.Build();
 
            // Localization
            var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            // Map gRPC services
            app.MapGrpcService<ExtensionService>().DisableGrpcWeb();
            app.MapGrpcService<RingGroupService>().DisableGrpcWeb();
            app.MapGrpcService<ContactService>().DisableGrpcWeb();
            app.MapGrpcService<QueueService>().DisableGrpcWeb();
            app.MapGrpcService<CallService>().DisableGrpcWeb();
            app.MapGrpcService<PbxEventListenerService>().DisableGrpcWeb();
            app.MapGrpcService<SqlService>().DisableGrpcWeb();
            app.MapGrpcService<IvrService>().DisableGrpcWeb();
            app.MapGrpcReflectionService();
            app.Run();
        }
        
    }
}