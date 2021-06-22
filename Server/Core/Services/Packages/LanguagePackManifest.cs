using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Helpers;
using Connect.LanguagePackManager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
    public class LanguagePackManifest : XmlDocument
    {
        public int ModuleId { get; set; }
        public int ManifestVersion { get; set; } = 0;
        public List<ComponentPack> Components { get; set; } = new List<ComponentPack>();

        public LanguagePackManifest(UnzipResult unzipResult, int moduleId)
        {
            this.ModuleId = moduleId;
            this.Load(Path.Combine(unzipResult.UnzipDirectory, unzipResult.ManifestFile));

            var mainNodes = this.SelectNodes("dotnetnuke/packages/package");
            if (mainNodes.Count > 0)
            {
                this.ManifestVersion = 5;
                this.LoadPackV5(unzipResult);
            }
            else
            {
                mainNodes = this.SelectNodes("dotnetnuke/folders/folder");
                if (mainNodes.Count > 0)
                {
                    this.ManifestVersion = 3;
                    this.LoadPackV3(unzipResult);
                }
            }
        }

        private void LoadPackV3(UnzipResult unzipResult)
        {
            // Not implemented
            throw new System.Exception("Version 3 manifest not implemented");
        }

        private void LoadPackV5(UnzipResult unzipResult)
        {
            foreach (XmlNode p in this.SelectNodes("dotnetnuke/packages/package"))
            {
                try
                {
                    string packType = p.SelectSingleNode("@type").InnerText.ToLowerInvariant();
                    var isCorePack = packType == "corelanguagepack";
                    var component = isCorePack ? Common.Globals.glbCoreName : p.SelectSingleNode("components/component/languageFiles/package").InnerText;

                    var package = PackageRepository.Instance.FindPackage(component, this.ModuleId);
                    if (package != null)
                    {
                        var comp = new ComponentPack()
                        {
                            Package = package,
                            Version = p.SelectSingleNode("@version").InnerText.ParseVersion().ToNormalizedFormat(),
                            Locale = p.SelectSingleNode("components/component/languageFiles/code").InnerText.ToLowerInvariant()
                        };

                        string basePath = "";
                        if (p.SelectSingleNode("components/component/languageFiles/basePath") is object)
                        {
                            basePath = p.SelectSingleNode("components/component/languageFiles/basePath").InnerText;
                        }

                        foreach (XmlNode xNode in p.SelectNodes("components/component/languageFiles/languageFile"))
                        {
                            string filePath = xNode.SelectSingleNode("path").InnerText;
                            string fileName = xNode.SelectSingleNode("name").InnerText;
                            string fileKey = Globals.DnnPathCombine(basePath, filePath, Regex.Replace(fileName, @"(?i)\.\w{2}(-\w+)?\.resx$(?-i)", ".resx"));

                            var fileInZip = Globals.DnnPathCombine(filePath, fileName);

                            foreach (var f in unzipResult.ResourceFiles.Values)
                            {
                                if (f.FilePathLowered.Replace("\\", "/").CompareTo(fileInZip) == 0)
                                {
                                    var rfile = new ResxFile(fileKey, Path.Combine(unzipResult.UnzipDirectory, f.HashedName));
                                    comp.ResourceFiles.Add(rfile);
                                }
                            }
                        }

                        this.Components.Add(comp);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

    }
}
