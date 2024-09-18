using PbxApiControl.Services.Grpc;
using PbxApiControl.Extensions;
using PbxApiControl.Config;
using PbxApiControl.Interceptor;
using PbxApiControl.Services;
using PbxApiControl.Interface;
using Serilog;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using PbxApiControl.Middleware;


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

            // Initialize configuration
            var configuration = ConfigService.GetConfiguration(builder);
            builder.Services.AddTransient<AuthMiddleware>();

            // Register TokenValidationService with configuration
            builder.Services.AddSingleton<ITokenValidationService>(sp => new TokenValidationService(configuration));
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
            builder.Services.AddApplicationServices(configuration);
            builder.Services.AddSingleton<IConfiguration>(configuration);
            builder.Services.AddHostedService<StartupService>();
            if (OperatingSystem.IsWindows())
            {
                builder.Services.AddHostedService<WindowsBackgroundService>();
            }

            // Configure Kestrel server
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(new Uri(configuration["Kestrel:EndpointDefaults:Url"]).Port, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });

            });
            
            var app = builder.Build();
            app.UseMiddleware<AuthMiddleware>();
 
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
            app.MapGrpcReflectionService();
            app.Run();
        }
        
    }
}