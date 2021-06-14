using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Helpers;
using Connect.LanguagePackManager.Core.Services.ResourceFiles;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public Dictionary<string, string> ListedResourceFiles { get; set; } = new Dictionary<string, string>();
        public UnzipResult ResourcesFile { get; set; }

        public void ParseFileNode(XmlNode fileNode, string basePath)
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
                    string resPath = resDir + resFile;
                    if (node["sourceFileName"] is object)
                        resPath = resDir + node["sourceFileName"].InnerText;
                    string resKey = basePath + resDir + resFile;
                    this.ListedResourceFiles.Add(resKey, resPath);
                }
            }
        }
    }
}
