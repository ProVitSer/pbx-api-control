using Serilog;
using Serilog.Events;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace PbxApiControl.Services
{
    public class LoggerService
    {
        public static void ConfigureLogger(string[] args)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            string logDirectory = Path.Combine(baseDirectory, "logs");
            
            Directory.CreateDirectory(logDirectory);
            
            string logFilePath = Path.Combine(logDirectory, "pacg-log-.txt");

            // Configuration constants
            const string applicationName = "pbx-call-control app";

            // Default log level
            LogEventLevel minimumLogLevel = LogEventLevel.Information;

            // Parse command-line arguments for log level
            if (args.Length > 0)
            {
                var logLevelArg = args.FirstOrDefault(arg => arg.StartsWith("--logLevel=", StringComparison.OrdinalIgnoreCase));
                
                if (logLevelArg != null)
                {
                    var logLevelString = logLevelArg.Split('=')[1];
                    if (Enum.TryParse(logLevelString, true, out LogEventLevel parsedLogLevel))
                    {
                        minimumLogLevel = parsedLogLevel;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid log level '{logLevelString}', using default '{minimumLogLevel}'");
                    }
                }
            }

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(minimumLogLevel)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("ApplicationName", applicationName)
                .WriteTo.Console(new ExpressionTemplate(
                    "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}",
                    theme: TemplateTheme.Code))
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }
    }
}

