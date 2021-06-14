using Newtonsoft.Json;
using System;
using System.IO;

namespace Connect.LanguagePackManager.Core.Common
{
    public class Globals
    {
        public const string glbCoreName = "Core";
        public const string glbCoreFriendlyName = "DNN Core";

        public static string GetTempFolder()
        {
            var res = Path.Combine(DotNetNuke.Common.Globals.HostMapPath, @"LPM\Temp");
            if (!Directory.Exists(res)) Directory.CreateDirectory(res);
            return res;
        }

        public static Version GetAssemblyVersion(string path)
        {
            try
            {
                string v = System.Diagnostics.FileVersionInfo.GetVersionInfo(path).FileVersion;
                return v.ParseVersion();
            }
            catch (Exception ex)
            {
                return new Version(0, 0, 0);
            }
        }

        public static void SaveObject(string filename, object objectToSave)
        {
            using (var sw = new StreamWriter(filename))
            {
                sw.WriteLine(JsonConvert.SerializeObject(objectToSave, Formatting.Indented));
            }
        }

        public static T GetObject<T>(string filename, T defaultObject)
        {
            T res = defaultObject;
            if (File.Exists(filename))
            {
                using (var sr = new StreamReader(filename))
                {
                    var list = sr.ReadToEnd();
                    res = JsonConvert.DeserializeObject<T>(list);
                }
            }
            return res;
        }

        public static string ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return "";
            }
            var res = "";
            using (var sr = new StreamReader(filePath))
            {
                res = sr.ReadToEnd();
            }
            return res;
        }
    }
}
