using PbxApiControl.Services.Grpc;
using PbxApiControl.Extensions;
using PbxApiControl.Config;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;
using PbxApiControl.Logging;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace PbxApiControl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunApplication(args);
        }
        
        public static void RunApplication(string[] args)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc(options =>
            {
                options.Interceptors.Add<LoggingInterceptor>();
            }).AddJsonTranscoding();
            
            // Initialize configuration
            PBXAPIConfig.InitConfig();
            
            builder.Services.AddGrpcReflection();
            
            builder.Services.AddApplicationServices();

            builder.Services.AddSerilog((services, lc) => lc
               .ReadFrom.Configuration(builder.Configuration)
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

            var app = builder.Build();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string kestrelUrl = configuration["Kestrel:EndpointDefaults:Url"];
            
            // Add URL bindings
            app.Urls.Add(kestrelUrl);

            // Localization
            var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            // Map gRPC services
            app.MapGrpcService<ExtensionService>();
            app.MapGrpcReflectionService();

            app.Run();
        }
    }
}