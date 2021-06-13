using Connect.LanguagePackManager.Core.Common;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
    public class ManifestPackage
    {
        public string PackageName { get; set; }
        public Version PackageVersion { get; set; }
        public string PackageType { get; set; }
        public string FriendlyName { get; set; }
        public string FolderName { get; set; }
        public SortedList ResourceFiles { get; set; } = new SortedList();

        public void ParseFileNode(XmlNode fileNode, string basePath, string tempDirectory)
        {
            foreach (XmlNode node in fileNode.SelectNodes("file"))
            {
                string resFile = "";
                string resDir = "";
                if (node["name"] is object)
                    resFile = node["name"].InnerText;
                if (node["path"] is object)
                    resDir = node["path"].InnerText;
                if (resFile.ToLower().EndsWith(".resx"))
                {
                    string resPath = Path.Combine(Path.Combine(tempDirectory, resDir), resFile);
                    if (node["sourceFileName"] is object)
                        resPath = Path.Combine(Path.Combine(tempDirectory, resDir), node["sourceFileName"].InnerText);
                    string resKey = Path.Combine(Path.Combine(basePath, resDir), resFile);
                    this.ResourceFiles.Add(resKey, new FileInfo(resPath));
                }
            }
        }

        public void ReadResourceFile(string keyBasePath, string path)
        {
            if (!string.IsNullOrEmpty(keyBasePath))
            {
                keyBasePath.EnsureEndsWith(@"\");
            }

            foreach (FileInfo f in new DirectoryInfo(path).GetFiles("*.resx"))
            {
                if (this.ResourceFiles[keyBasePath + f.Name] is null)
                {
                    var m = Regex.Match(f.Name, @"\.(\w{2,3}-\w\w)\.");
                    if (!m.Success || m.Groups[1].Value.ToLower() == "en-us") // filter out all files that are not default locale
                    {
                        this.ResourceFiles.Add(keyBasePath + f.Name, f);
                    }
                }
            }

            foreach (string d in Directory.GetDirectories(path))
                this.ReadResourceFile(keyBasePath + d.Substring(d.LastIndexOf(@"\") + 2), d);
        }

    }
}
