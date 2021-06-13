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
            return Path.Combine(DotNetNuke.Common.Globals.HostMapPath, @"LPM\Temp");
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
                return new Version(0,0,0);
            }
        }
    }
}
