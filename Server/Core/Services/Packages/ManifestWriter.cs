using Connect.LanguagePackManager.Core.Common;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
    public class ManifestWriter : XmlDocument
    {
        public CultureInfo Locale { get; set; }

        public List<PackageWriter> Packages { get; set; } = new List<PackageWriter>();

        public ManifestWriter(CultureInfo loc)
        {
            this.Locale = loc;
        }

        public void Compile()
        {
            this.CreateXmlDeclaration("1.0", null, null);
            var root = this.CreateElement("dotnetnuke");
            this.AppendChild(root);
            root.AddAttribute("type", "Package");
            root.AddAttribute("version", "5.0");
            var packagesNode = root.AddChildElement("packages");
            foreach (var package in this.Packages)
            {
                var packageNode = packagesNode.AddChildElement("package");
                packageNode.AddAttribute("name", package.PackageName + "_" + this.Locale.Name);
                packageNode.AddAttribute("type", package.IsCore ? "CoreLanguagePack" : "ExtensionLanguagePack");
                packageNode.AddAttribute("version", package.PackageVersion);
                packageNode.AddChildElement("friendlyName", package.FriendlyName + " " + this.Locale.NativeName);
                packageNode.AddChildElement("description", $"{this.Locale.EnglishName} language pack for {package.FriendlyName}");
                var owner = packageNode.AddChildElement("owner");
                owner.AddChildElement("name", "");
                owner.AddChildElement("organization", "");
                owner.AddChildElement("url", "");
                owner.AddChildElement("email", "");
                var component = packageNode.AddChildElement("components").AddChildElement("component");
                component.AddAttribute("type", package.IsCore ? "CoreLanguage" : "ExtensionLanguage");
                var files = component.AddChildElement("languageFiles");
                files.AddChildElement("code", this.Locale.Name);
                files.AddChildElement("displayName", this.Locale.NativeName);
                if (!package.IsCore)
                {
                    files.AddChildElement("package", package.PackageName); // this creates the dependency between lang pack and object (ignored for Core)
                }
                files.AddChildElement("basePath", "");
                foreach (var filePath in package.ResourceFiles)
                {
                    var file = files.AddChildElement( "languageFile");
                    file.AddChildElement( "path", filePath);
                    file.AddChildElement("name", Path.GetFileName(filePath));
                }
            }
        }

        public void AddPackage(string packageName, string friendlyName, string version, List<string> resourceFiles)
        {
            var newPackage = new PackageWriter()
            {
                PackageName = packageName,
                FriendlyName = friendlyName,
                IsCore = packageName == Common.Globals.glbCoreName,
                PackageVersion = version,
                ResourceFiles = resourceFiles
            };
            this.Packages.Add(newPackage);
        }

        public class PackageWriter
        {
            public string PackageName { get; set; }
            public string PackageVersion { get; set; }
            public string FriendlyName { get; set; }
            public bool IsCore { get; set; }
            public List<string> ResourceFiles { get; set; }
        }
    }
}
