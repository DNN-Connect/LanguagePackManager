using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Connect.LanguagePackManager.Core.Common
{
    public class Globals
    {
        public const string glbCoreName = "Core";
        public const string glbCoreFriendlyName = "DNN Core";

        public static string GetLpmFolder(int portalId, string subFolder)
        {
            var res = Path.Combine(GetLpmFolder(portalId), subFolder);
            if (!Directory.Exists(res)) Directory.CreateDirectory(res);
            return res;
        }

        public static string GetLpmFolder(int portalId)
        {
            var res = portalId == -1 ? DotNetNuke.Common.Globals.HostMapPath : DotNetNuke.Entities.Portals.PortalController.Instance.GetPortal(portalId).HomeDirectoryMapPath;
            res = Path.Combine(res, "LPM");
            if (!Directory.Exists(res)) Directory.CreateDirectory(res);
            return res;
        }

        public static void CleanupTempFolder()
        {
            var tempFolder = new DirectoryInfo(GetLpmFolder(-1, "Temp"));
            if (tempFolder.Exists)
            {
                foreach (var dir in tempFolder.GetDirectories())
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
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

        public static string DnnPathCombine(string path1, params string[] otherPaths)
        {
            var paths = new List<string>();
            paths.Add(path1.Replace("\\", "/").Trim('/').ToLowerInvariant());
            foreach (var p in otherPaths)
            {
                paths.Add(p.Replace("\\", "/").Trim('/').ToLowerInvariant());
            }
            return string.Join("/", paths).Trim('/');
        }
    }
}
