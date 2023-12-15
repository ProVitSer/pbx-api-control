using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace PbxApiControl
{
    public class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Проблемы запуска проекта");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    PBXAPIConfig.InitConfig();
                    PbxEventListener.Start();
                });
    }
}