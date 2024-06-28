using System.Reflection;
using TCX.Configuration;

namespace PbxApiControl.Config
{
    public static class PbxApiConfig
    {
        public static string? InstanceBinPath;
        
        private static readonly string pbxIniPath = GetPbxIniPath();

        public static void InitConfig()
        {
            try
            {
                
                var content = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
                
                ReadConfiguration(content, pbxIniPath);

                InstanceBinPath = Path.Combine(content["General"]["AppPath"], "Bin");

                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => CurrentDomain_AssemblyResolve(args, InstanceBinPath);

                ConnectToTcx(content);
            }
            catch (Exception ex)
            {
                throw new Exception("Проблемы с инициализацией: " + ex.Message, ex);
            }
        }

        private static void ConnectToTcx(Dictionary<string, Dictionary<string, string>> content)
        {
            try
            {
                PhoneSystem.CfgServerHost = "127.0.0.1";
                PhoneSystem.CfgServerPort = int.Parse(content["ConfService"]["ConfPort"]);
                PhoneSystem.CfgServerUser = content["ConfService"]["confUser"];
                PhoneSystem.CfgServerPassword = content["ConfService"]["confPass"];
                var ps = PhoneSystem.Reset(
                  PhoneSystem.ApplicationName + new Random(Environment.TickCount).Next().ToString(),
                  "127.0.0.1",
                  int.Parse(content["ConfService"]["ConfPort"]),
                  content["ConfService"]["confUser"],
                  content["ConfService"]["confPass"]);

                ps.WaitForConnect(TimeSpan.FromSeconds(5.0));
            }
            catch (Exception ex)
            {
                throw new Exception("Проблемы с подключение к АТС: " + ex.Message, ex);
            }
        }

        private static void ReadConfiguration(
          Dictionary<string, Dictionary<string, string>> content,
          string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Не удается найти " + Path.GetFullPath(filePath));

            string[] strArray = File.ReadAllLines(filePath);

            Dictionary<string, string> dictionary = null;

            foreach (var str in strArray)
            {
                string trimmedStr = str.Trim();

                if (trimmedStr.StartsWith("["))
                {
                    string section = trimmedStr.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    dictionary = content[section] = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                }
                else if (dictionary != null && !string.IsNullOrWhiteSpace(trimmedStr) && !trimmedStr.StartsWith("#") && !trimmedStr.StartsWith(";"))
                {
                    string[] array = trimmedStr.Split("=", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                    dictionary[array[0]] = array[1];
                }
            }
        }

        private static Assembly? CurrentDomain_AssemblyResolve(ResolveEventArgs args, string? instanceBinPath)
        {
            var name = new AssemblyName(args.Name).Name;

            var assemblyPath = Path.Combine(instanceBinPath ?? "", name + ".dll");

            if (File.Exists(assemblyPath))
            {
                Console.WriteLine($"Assembly found at path: {assemblyPath}");
                return Assembly.LoadFrom(assemblyPath);
            }
            else
            {
                Console.WriteLine($"Assembly not found at path: {assemblyPath}");
                return null;
            }
        }
        
        private static string GetPbxIniPath()
        {
            if (OperatingSystem.IsWindows())
            {
                return @"C:\Program Files\3CX Phone System\Bin\3CXPhoneSystem.ini";
            }
            else if (OperatingSystem.IsLinux())
            {
                return "/var/lib/3cxpbx/Bin/3CXPhoneSystem.ini";
            }
            else
            {
                throw new PlatformNotSupportedException("Unsupported OS");
            }
        }
    }
}