using PbxApiControl.Services;
using PbxApiControl.Extensions;
using Microsoft.AspNetCore.Hosting;

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


            var app = builder.Build();
            app.Urls.Add("http://192.168.0.2:5000");
            app.MapGrpcService<GreeterService>();
            app.MapGrpcService<TestService>();

            app.MapGrpcReflectionService();
            PBXAPIConfig.InitConfig();

            app.Run();
        }
        
    }
}
