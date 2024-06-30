using PbxApiControl.Services.Grpc;
using PbxApiControl.Extensions;
using PbxApiControl.Config;
using PbxApiControl.Interceptor;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;

namespace PbxApiControl
{
    public class Program
    {
        private const string DevelopmentEnvironment = "Development";
        private const string GrpcUrlKey = "Kestrel:Endpoints:grpc:Url";
        private const string SwaggerUrlKey = "Kestrel:Endpoints:swagger:Url";
        
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            SetCulture(builder);
            
            var configuration = GetConfiguration(builder);
            
            string grpcUrl = configuration[GrpcUrlKey]  ?? throw new ArgumentException("Invalid host port args");
            string swaggerUrl = configuration[SwaggerUrlKey]  ?? throw new ArgumentException("Invalid host port args");

            // Add services to the container.
            builder.Services.AddGrpc(options =>
                {
                    options.Interceptors.Add<LoggingInterceptor>();
                })
                .AddJsonTranscoding(); // Add support for JSON transcoding

            if (builder.Environment.EnvironmentName == "Development")
            {
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "gRPC JSON transcoding example", Version = "v1" });
                
                    var filePath = Path.Combine(System.AppContext.BaseDirectory, "Server.xml");
                    c.IncludeXmlComments(filePath);
                    c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
                });
                builder.Services.AddGrpcSwagger();  
            }
            
            // Initialize configuration
            PbxApiConfig.InitConfig();
            
            builder.Services.AddGrpcReflection();
            builder.Services.AddApplicationServices();
            builder.Services.AddSingleton(configuration);

            builder.Services.AddSerilog((services, lc) => lc
               .ReadFrom.Configuration(configuration)
               .ReadFrom.Services(services)
               .Enrich.FromLogContext()
               .WriteTo.Console(new ExpressionTemplate(
                   "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}",
                   theme: TemplateTheme.Code)));
            
            // Configure Kestrel server
            builder.WebHost.ConfigureKestrel(options =>
            {
                int swaggerPort = new Uri(swaggerUrl).Port;
                int grpcPort = new Uri(grpcUrl).Port;

                options.ListenAnyIP(swaggerPort, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1;
                });

                options.ListenAnyIP(grpcPort, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });

            });
            
            var app = builder.Build();

            if (builder.Environment.EnvironmentName == "Development")
            {
                app.UseDefaultFiles();
                app.UseStaticFiles();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "gRPC 3CX PBX CONTROL API V1");
                });

            }

            app.Use(async (context, next) =>
            {
                if (context.Request.ContentType == "application/grpc")
                {
                    await next();
                }
                else if (builder.Environment.EnvironmentName != DevelopmentEnvironment)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                else
                {
                    await next();
                }
            });

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
            app.MapGrpcReflectionService();
            app.Run();
        }
        
        private static void SetCulture(WebApplicationBuilder builder)
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

        private static IConfigurationRoot GetConfiguration(WebApplicationBuilder builder)
        {
            Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

            string appsettingsFile = builder.Environment.EnvironmentName == DevelopmentEnvironment
                ? $"PbxApiControl.appsettings.{builder.Environment.EnvironmentName}.json"
                : "PbxApiControl.appsettings.json";
    
            return new ConfigurationBuilder()
                .AddJsonStream(GetEmbeddedResourceStream(appsettingsFile))
                .Build();
        }


        private static Stream GetEmbeddedResourceStream(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream = assembly.GetManifestResourceStream(resourceName);

            if (resourceStream == null)
            {
                throw new FileNotFoundException("Embedded resource not found", resourceName);
            }

            return resourceStream;
        }
    }
}