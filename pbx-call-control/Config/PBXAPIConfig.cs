using System.Reflection;
using TCX.Configuration;

namespace PbxApiControl.Config
{
    public static class PBXAPIConfig
    {
        public static string? instanceBinPath;

        public static void InitConfig()
        {
            try
            {
                Dictionary<string, Dictionary<string, string>> iniContent = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);
                Dictionary<string, Dictionary<string, string>> Content = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);

                ReadConfiguration(iniContent, "api.ini");
                ReadConfiguration(Content, Path.Combine(iniContent["General"]["PBX_INI_PATH"], "3CXPhoneSystem.ini"));

                instanceBinPath = Path.Combine(Content["General"]["AppPath"], "Bin");

                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => CurrentDomain_AssemblyResolve(args, instanceBinPath);

                ConnectToTCX(Content);
            }
            catch (Exception ex)
            {
                throw new Exception("Проблемы с инициализацией: " + ex.Message, ex);
            }
        }

        private static void ConnectToTCX(Dictionary<string, Dictionary<string, string>> Content)
        {
            try
            {
                PhoneSystem.CfgServerHost = "127.0.0.1";
                PhoneSystem.CfgServerPort = int.Parse(Content["ConfService"]["ConfPort"]);
                PhoneSystem.CfgServerUser = Content["ConfService"]["confUser"];
                PhoneSystem.CfgServerPassword = Content["ConfService"]["confPass"];
                var ps = PhoneSystem.Reset(
                  PhoneSystem.ApplicationName + new Random(Environment.TickCount).Next().ToString(),
                  "127.0.0.1",
                  int.Parse(Content["ConfService"]["ConfPort"]),
                  Content["ConfService"]["confUser"],
                  Content["ConfService"]["confPass"]);

                ps.WaitForConnect(TimeSpan.FromSeconds(5.0));
            }
            catch (Exception ex)
            {
                throw new Exception("Проблемы с подключение к АТС: " + ex.Message, ex);
            }
        }

        private static void ReadConfiguration(
          Dictionary<string, Dictionary<string, string>> Content,
          string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Не удается найти " + Path.GetFullPath(filePath));

            string[] strArray = File.ReadAllLines(filePath);

            Dictionary<string, string> dictionary = null;

            for (int index = 1; index < strArray.Length + 1; ++index)
            {
                string str1 = strArray[index - 1].Trim();

                if (str1.StartsWith("["))
                {
                    string str2 = str1.Split(new char[2] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    dictionary = Content[str2] = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                }
                else if (dictionary != null && !string.IsNullOrWhiteSpace(str1) && !str1.StartsWith("#") && !str1.StartsWith(";"))
                {
                    string[] array = str1.Split("=", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                    dictionary[array[0]] = array[1];
                }
            }
        }

        static Assembly CurrentDomain_AssemblyResolve(ResolveEventArgs args, string? instanceBinPath)
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
    }
}