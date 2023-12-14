using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using TCX.Configuration;
using TCX.PBXAPI;

public static class PBXAPIConfig
{
    public static string LOG_PATH;
    public static long MAX_LOG_FILE_SIZE;
    public static uint MAX_LOG_RECORDS;
    public static string MS_HOST = "localhost:5233";
    private static readonly string instanceBinPath;



    static PBXAPIConfig()
    {
        try
        {
            Dictionary<string, Dictionary<string, string>> iniContent = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);
            PBXAPIConfig.ReadConfiguration(iniContent, "Api.ini");
            InitProjConf(iniContent);


            Dictionary<string, Dictionary<string, string>> Content = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>)StringComparer.InvariantCultureIgnoreCase);
            PBXAPIConfig.ReadConfiguration(Content, Path.Combine(iniContent["General"]["PBX_INI_PATH"], "3CXPhoneSystem.ini"));
            PBXAPIConfig.instanceBinPath = Path.Combine(Content["General"]["AppPath"], "Bin");
            Console.WriteLine(PBXAPIConfig.instanceBinPath);

            AppDomain.CurrentDomain.AssemblyResolve += PBXAPIConfig.CurrentDomain_AssemblyResolve;



            ConnectToTCX(Content);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

        }

    }

    private static void InitProjConf(Dictionary<string, Dictionary<string, string>> IniContent)
    {
        PBXAPIConfig.LOG_PATH = IniContent["General"][nameof(LOG_PATH)];
        PBXAPIConfig.MAX_LOG_FILE_SIZE = long.Parse(IniContent["General"][nameof(MAX_LOG_FILE_SIZE)]);
        PBXAPIConfig.MAX_LOG_RECORDS = uint.Parse(IniContent["General"][nameof(MAX_LOG_RECORDS)]);
    }

    private static void ConnectToTCX(Dictionary<string, Dictionary<string, string>> Content)
    {
        try
        {
            Console.WriteLine(int.Parse(Content["ConfService"]["ConfPort"]));
            Console.WriteLine(Content["ConfService"]["confUser"]);
            Console.WriteLine(Content["ConfService"]["confPass"]);

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
            ps.WaitForConnect(TimeSpan.FromSeconds(30));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Stopped program because of exception");

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

    private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {

        string name = new AssemblyName(args.Name).Name;

        try
        {

            return Assembly.LoadFrom(Path.Combine(PBXAPIConfig.instanceBinPath, name + ".dll"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return (Assembly)null;
        }
    }
}