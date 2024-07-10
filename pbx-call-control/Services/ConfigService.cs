using System.Reflection;

namespace PbxApiControl.Services
{
    public class ConfigService
    {
        private const string DevelopmentEnvironment = "Development";

        public static IConfigurationRoot GetConfiguration(WebApplicationBuilder builder)
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

