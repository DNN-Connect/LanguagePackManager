using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
    public class Manifest : XmlDocument
    {
        public int ManifestVersion { get; set; } = 0;
        public List<ManifestPackage> Packages { get; set; } = new List<ManifestPackage>();

        public Manifest(UnzipResult unzipResult)
        {
            this.Load(Path.Combine(unzipResult.UnzipDirectory, unzipResult.ManifestFile));

            var mainNodes = this.SelectNodes("dotnetnuke/packages/package");
            if (mainNodes.Count > 0)
            {
                this.ManifestVersion = 5;
            }
            else
            {
                mainNodes = this.SelectNodes("dotnetnuke/folders/folder");
                if (mainNodes.Count > 0)
                {
                    this.ManifestVersion = 3;
                }
            }

            if (this.ManifestVersion == 5)
            {
                // Remark about DNN 5 manifest file: it is assumed that only one <desktopModule> node per <package> node exists.

                // Create a module for each package
                foreach (XmlNode packageNode in mainNodes)
                {
                    var manifestModule = new ManifestPackage();
                    manifestModule.PackageVersion = packageNode.SelectSingleNode("@version").InnerText.Trim().ParseVersion();
                    manifestModule.PackageName = packageNode.SelectSingleNode("@name").InnerText.Trim();
                    if (packageNode["friendlyName"] is object)
                    {
                        manifestModule.FriendlyName = packageNode["friendlyName"].InnerText;
                    }
                    else
                    {
                        manifestModule.FriendlyName = manifestModule.PackageName;
                    }
                    manifestModule.PackageType = packageNode.SelectSingleNode("@type").InnerText;

                    switch (manifestModule.PackageType.ToLower() ?? "")
                    {
                        case "module":
                            {

                                // Determine the desktop module
                                var moduleNodes = packageNode.SelectNodes("components/component[@type='Module']/desktopModule");
                                foreach (XmlNode dmNode in moduleNodes)
                                {
                                    manifestModule.FolderName = dmNode["foldername"].InnerText.Replace('/', '\\');
                                }

                                // Find the resource files using the manifest xml
                                foreach (XmlNode fileGroupNode in packageNode.SelectNodes("components/component[@type='File']/files"))
                                {
                                    string basePath = Path.Combine("DesktopModules", manifestModule.FolderName);
                                    if (fileGroupNode["basePath"] is object)
                                    {
                                        basePath = fileGroupNode["basePath"].InnerText.Replace('/', '\\').Trim('\\');
                                    }

                                    manifestModule.ParseFileNode(fileGroupNode, basePath);
                                }

                                break;
                            }

                        case "skin":
                            {

                                // Find the resource files using the manifest xml
                                foreach (XmlNode fileGroupNode in packageNode.SelectNodes("components/component[@type='Skin']/skinFiles"))
                                {
                                    string basePath = "";
                                    if (fileGroupNode["basePath"] is object)
                                    {
                                        basePath = fileGroupNode["basePath"].InnerText.Replace('/', '\\').Trim('\\');
                                    }
                                    manifestModule.ParseFileNode(fileGroupNode, basePath);
                                }

                                break;
                            }

                        case "container":
                            {

                                // Find the resource files using the manifest xml
                                foreach (XmlNode fileGroupNode in packageNode.SelectNodes("components/component[@type='Container']/containerFiles"))
                                {
                                    string basePath = "";
                                    if (fileGroupNode["basePath"] is object)
                                    {
                                        basePath = fileGroupNode["basePath"].InnerText.Replace('/', '\\').Trim('\\');
                                    }
                                    manifestModule.ParseFileNode(fileGroupNode, basePath);
                                }

                                break;
                            }

                        default:
                            {
                                foreach (XmlNode fileGroupNode in packageNode.SelectNodes("components/component[@type='File']/files"))
                                {
                                    string basePath = "";
                                    if (fileGroupNode["basePath"] is object)
                                    {
                                        basePath = fileGroupNode["basePath"].InnerText.Replace('/', '\\').Trim('\\');
                                    }
                                    manifestModule.ParseFileNode(fileGroupNode, basePath);
                                }

                                break;
                            }
                    }

                    // Handle resource files
                    foreach (XmlNode resFileNode in packageNode.SelectNodes("components/component[@type='ResourceFile']"))
                    {
                        string basePath = resFileNode.SelectSingleNode("resourceFiles/basePath").InnerText.Replace('/', '\\').Trim('\\');
                        string resFile = resFileNode.SelectSingleNode("resourceFiles/resourceFile/name").InnerText.ToLowerInvariant();

                        // find the zip file
                        foreach (var zipFile in unzipResult.ZipFiles.Values)
                        {
                            if (zipFile.FilePathLowered == resFile)
                            {
                                manifestModule.ResourcesFile = ZipHelper.Unzip(Path.Combine(unzipResult.UnzipDirectory, zipFile.HashedName), basePath);
                            }
                        }
                    }

                    // Add the manifest module to the collection
                    this.Packages.Add(manifestModule);
                }
            }
            else if (this.ManifestVersion == 3)
            {

                // Create a module for each folder
                foreach (XmlNode folderNode in mainNodes)
                {
                    var manifestModule = new ManifestPackage();
                    // manifestModule.DnnCoreVersion = DnnCoreVersion

                    // Determine the module name
                    if (folderNode["modulename"] is object)
                    {
                        manifestModule.PackageName = folderNode["modulename"].InnerText;
                    }
                    else if (folderNode["friendlyname"] is object)
                    {
                        manifestModule.PackageName = folderNode["friendlyname"].InnerText;
                    }
                    else if (folderNode["name"] is object)
                    {
                        manifestModule.PackageName = folderNode["name"].InnerText;
                    }
                    else
                    {
                        throw new Exception("Could not retrieve module name in DNN Manifest file");
                    }

                    manifestModule.PackageType = this.SelectSingleNode("dotnetnuke/@type").InnerText;

                    // Determine the friendly name
                    if (folderNode["friendlyname"] is object)
                    {
                        manifestModule.FriendlyName = folderNode["friendlyname"].InnerText;
                    }
                    else
                    {
                        manifestModule.FriendlyName = manifestModule.PackageName;
                    }

                    manifestModule.FolderName = folderNode["foldername"].InnerText.Replace('/', '\\');
                    manifestModule.PackageVersion = folderNode["version"].InnerText.Trim().ParseVersion();

                    // Find the resource files using the manifest xml
                    foreach (XmlNode node in folderNode.SelectNodes("files/file"))
                    {
                        string resFile = "";
                        string resDir = "";
                        if (node["name"] is object)
                            resFile = node["name"].InnerText;
                        if (node["path"] is object)
                            resDir = node["path"].InnerText;

                        // TODO Support resource files which are already localized
                        if (resFile.ToLower().EndsWith("ascx.resx") || resFile.ToLower().EndsWith("aspx.resx"))
                        {
                            // Determine the resource directory and key for the module
                            string resPath = resDir + resFile;
                            string resKey = Path.Combine("DesktopModules", manifestModule.FolderName, resDir, resFile);
                            manifestModule.ListedResourceFiles.Add(resKey, resPath);
                        }
                    }

                    // Find the resource file
                    if (folderNode["resourcefile"] is object)
                    {
                        string basePath = @"DesktopModules\" + manifestModule.FolderName;
                        string resFile = folderNode["resourcefile"].InnerText.ToLowerInvariant();

                        // find the zip file
                        foreach (var zipFile in unzipResult.ZipFiles.Values)
                        {
                            if (zipFile.FilePathLowered == resFile)
                            {
                                try
                                {
                                    manifestModule.ResourcesFile = ZipHelper.Unzip(Path.Combine(unzipResult.UnzipDirectory, zipFile.HashedName), basePath);
                                }
                                catch (Exception ex) 
                                {
                                    // ignoring for now
                                }
                            }
                        }
                    }

                    // Add the manifest module to the collection
                    this.Packages.Add(manifestModule);
                }
            }

        }


    }
}
