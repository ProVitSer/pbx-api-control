using System.Reflection;
using TCX.Configuration;

namespace PbxApiControl
{

    public static class PBXAPIConfig
    {
        public static string instanceBinPath;



        public static void InitConfig()
        {
            try
            {
                Dictionary<string, Dictionary<string, string>> iniContent = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);
                Dictionary<string, Dictionary<string, string>> Content = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);


                PBXAPIConfig.ReadConfiguration(iniContent, "Api.ini");
                
                PBXAPIConfig.ReadConfiguration(Content, Path.Combine(iniContent["General"]["PBX_INI_PATH"], "3CXPhoneSystem.ini"));


                PBXAPIConfig.instanceBinPath = Path.Combine(Content["General"]["AppPath"], "Bin");

                AppDomain.CurrentDomain.AssemblyResolve += PBXAPIConfig.CurrentDomain_AssemblyResolve;
                Console.WriteLine(Path.Combine(Content["General"]["AppPath"], "Bin"));

                ConnectToTCX(Content);


            }
            catch (Exception ex)

            {
                Console.WriteLine(ex);

                throw new Exception("Проблемы с инициализацией");
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
            catch
            {
                throw new Exception("Проблемы с подключение к АТС");
            }
        }

        private static void ReadConfiguration(
        Dictionary<string, Dictionary<string, string>> Content,
        string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Не удается найти " + Path.GetFullPath(filePath));
            string[] strArray = File.ReadAllLines(filePath);

            Dictionary<string, string> dictionary = (Dictionary<string, string>)null;
            for (int index = 1; index < strArray.Length + 1; ++index)
            {
                string str1 = strArray[index - 1].Trim();
                if (str1.StartsWith("["))
                {
                    string str2 = str1.Split(new char[2] { '[', ']' }, (StringSplitOptions)1)[0];
                    dictionary = Content[str2] = new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);
                }
                else if (dictionary != null && !string.IsNullOrWhiteSpace(str1) && !str1.StartsWith("#") && !str1.StartsWith(";"))
                {
                    string[] array = Enumerable.ToArray<string>(Enumerable.Select<string, string>((IEnumerable<string>)str1.Split("=", (StringSplitOptions)0), (Func<string, string>)(x => x.Trim())));
                    dictionary[array[0]] = array[1];
                }
            }
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name).Name;
            try
            {
                return Assembly.LoadFrom(Path.Combine(instanceBinPath, name + ".dll"));
            }
            catch
            {
                return null;
            }
        }
    }
}
