using PbxApiControl.Services.Grpc;
using PbxApiControl.Extensions;
using PbxApiControl.Config;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace PbxApiControl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc().AddJsonTranscoding();

            builder.Services.AddGrpcReflection();

            builder.Services.AddApplicationServices();

            builder.Services.AddSerilog((services, lc) => lc
           .ReadFrom.Configuration(builder.Configuration)
           .ReadFrom.Services(services)
           .Enrich.FromLogContext()
           .WriteTo.Console(new ExpressionTemplate(
               "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}",
           theme: TemplateTheme.Code)));

            var app = builder.Build();

            app.Urls.Add("http://192.168.0.2:5000");

            app.MapGrpcService<ExtensionService>();

            app.MapGrpcReflectionService();

            PBXAPIConfig.InitConfig();

            app.Run();
        }

    }
}
