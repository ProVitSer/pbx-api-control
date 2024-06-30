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
        public static void Main(string[] args)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            
            var builder = WebApplication.CreateBuilder(args);
            
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var appsettings = environmentName == "Development"
                ? $"PbxApiControl.appsettings.{environmentName}.json"
                : "PbxApiControl.appsettings.json";
            
            var configuration = new ConfigurationBuilder()
                .AddJsonStream(GetEmbeddedResourceStream(appsettings))
                .Build();
            
            string kestrelUrl = configuration["Kestrel:EndpointDefaults:Url"]  ?? "http://127.0.0.1:5000";

            // Add services to the container.
            builder.Services.AddGrpc(options =>
                {
                    options.Interceptors.Add<LoggingInterceptor>();
                })
                .AddJsonTranscoding(); // Add support for JSON transcoding


            if (environmentName == "Development")
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

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("en-US") };
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });


            // Configure Kestrel server
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(new Uri(kestrelUrl).Port, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2; // Allow both HTTP/1.1 and HTTP/2
                });
            });
            
            var app = builder.Build();
            

            if (environmentName == "Development")
            {
                app.UseDefaultFiles();
                app.UseStaticFiles();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "gRPC 3CX PBX CONTROL API V1");
                });
            }
   
            
            // Add URL bindings
            app.Urls.Add(kestrelUrl);

            // Localization
            var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            // Map gRPC services
            app.MapGrpcService<ExtensionService>().EnableGrpcWeb();
            app.MapGrpcService<RingGroupService>().EnableGrpcWeb();
            app.MapGrpcService<ContactService>().EnableGrpcWeb();
            app.MapGrpcService<QueueService>().EnableGrpcWeb();
            app.MapGrpcService<CallService>().EnableGrpcWeb();
            app.MapGrpcService<PbxEventListenerService>().EnableGrpcWeb();

            app.MapGrpcReflectionService();
            
            app.Run();
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
