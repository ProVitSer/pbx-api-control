using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TCX.Configuration;

namespace PbxApiControl
{
    public static class PBXAPIConfig
    {
        public static string? LOG_PATH;
        public static string? LOG_NAME;
        public static string? BACKUP_LOG_NAME;
        public static long MAX_LOG_FILE_SIZE;
        public static uint MAX_LOG_RECORDS;
        public static string? instanceBinPath;

        public static void InitConfig()
        {
            try
            {
                Dictionary<string, Dictionary<string, string>> iniContent = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);
                Dictionary<string, Dictionary<string, string>> Content = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);

                ReadConfiguration(iniContent, "Api.ini");
                InitLoggerConf(iniContent);

                ReadConfiguration(Content, Path.Combine(iniContent["General"]["PBX_INI_PATH"], "3CXPhoneSystem.ini"));

                instanceBinPath = Path.Combine(Content["General"]["AppPath"], "Bin");

                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                Console.WriteLine(Path.Combine(Content["General"]["AppPath"], "Bin"));

                ConnectToTCX(Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Проблемы с инициализацией");
            }
        }

        private static void InitLoggerConf(Dictionary<string, Dictionary<string, string>> IniContent)
        {
            LOG_PATH = IniContent["General"][nameof(LOG_PATH)];
            LOG_NAME = IniContent["General"][nameof(LOG_NAME)];
            BACKUP_LOG_NAME = IniContent["General"][nameof(BACKUP_LOG_NAME)];
            MAX_LOG_FILE_SIZE = long.Parse(IniContent["General"][nameof(MAX_LOG_FILE_SIZE)]);
            MAX_LOG_RECORDS = uint.Parse(IniContent["General"][nameof(MAX_LOG_RECORDS)]);
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

            Dictionary<string, string>? dictionary = null;
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

        static Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name).Name;
            try
            {
                return Assembly.LoadFrom(Path.Combine(instanceBinPath!, name + ".dll"));
            }
            catch
            {
                return null;
            }
        }
    }
}