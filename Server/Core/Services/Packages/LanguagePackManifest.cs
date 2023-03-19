using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Data;
using Connect.LanguagePackManager.Core.Helpers;
using Connect.LanguagePackManager.Core.Repositories;
using Connect.LanguagePackManager.Core.Services.Github;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
  public class LanguagePackManifest : XmlDocument
  {
    public int ModuleId { get; set; }
    public int ManifestVersion { get; set; } = 0;
    public List<ComponentPack> Components { get; set; } = new List<ComponentPack>();
    private string BasePath { get; set; } = "";

    public LanguagePackManifest(GithubTree tree, GithubFile manifestFile, int moduleId)
    {
      var manifest = GithubService.DownloadFile(manifestFile.Url);

      this.ModuleId = moduleId;
      this.LoadXml(manifest);

      var mainNodes = this.SelectNodes("dotnetnuke/packages/package");
      if (mainNodes.Count > 0)
      {
        this.BasePath = Path.GetDirectoryName(manifestFile.Path);
        this.ManifestVersion = 5;
        this.LoadPackV5FromGithub(tree);
      }
      else
      {
        mainNodes = this.SelectNodes("dotnetnuke/folders/folder");
        if (mainNodes.Count > 0)
        {
          this.ManifestVersion = 3;
          throw new System.Exception("Version 3 manifest not implemented");
        }
      }
    }

    public LanguagePackManifest(UnzipResult unzipResult, int moduleId)
    {
      this.ModuleId = moduleId;
      this.Load(Path.Combine(unzipResult.UnzipDirectory, unzipResult.ManifestFile));

      var mainNodes = this.SelectNodes("dotnetnuke/packages/package");
      if (mainNodes.Count > 0)
      {
        this.ManifestVersion = 5;
        this.LoadPackV5FromUploadedZipFile(unzipResult);
      }
      else
      {
        mainNodes = this.SelectNodes("dotnetnuke/folders/folder");
        if (mainNodes.Count > 0)
        {
          this.ManifestVersion = 3;
          throw new System.Exception("Version 3 manifest not implemented");
        }
      }
    }

    public void Process(bool generic, int userId)
    {
      foreach (var p in this.Components)
      {
        var locale = generic ? p.Locale.Substring(0, 2) : p.Locale;
        var localeId = LocaleRepository.Instance.GetOrCreateLocale(locale).LocaleId;
        var dbResFiles = ResourceFileRepository.Instance.GetResourceFilesByPackage(p.Package.PackageId);
        foreach (var rf in p.ResourceFiles)
        {
          var dbFile = dbResFiles.FirstOrDefault(f => f.FilePath == rf.FileKey);
          if (dbFile != null)
          {
            var dbTexts = TextRepository.Instance.GetTextsByResourceFile(dbFile.ResourceFileId);
            foreach (var tt in rf.Resources.Keys)
            {
              var dbT = dbTexts.FirstOrDefault(t => t.TextKey == tt && t.CoversVersion(p.Version));
              if (dbT != null)
              {
                Sprocs.SetTranslation(dbT.TextId, localeId, rf.Resources[tt], userId);
              }
            }
          }
        }
      }
    }

    private void LoadPackV5FromUploadedZipFile(UnzipResult unzipResult)
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
            if (comp.Version == "00.00.00")
            {
              comp.Version = package.LastVersion;
            }

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
                  var fileContents = File.ReadAllText(Path.Combine(unzipResult.UnzipDirectory, f.HashedName));
                  var rfile = new ResxFile(fileKey, fileContents);
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

    private void LoadPackV5FromGithub(GithubTree tree)
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
            if (comp.Version == "00.00.00")
            {
              comp.Version = package.LastVersion;
            }

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

              var fileInZip = Globals.DnnPathCombine(this.BasePath, filePath, fileName).ToLowerInvariant();

              var fileOnGithub = tree.Files.FirstOrDefault(f => f.Path.ToLowerInvariant().CompareTo(fileInZip) == 0);
              if (fileOnGithub != null)
              {
                var rfile = new ResxFile(fileKey, GithubService.DownloadFile(fileOnGithub.Url));
                comp.ResourceFiles.Add(rfile);
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
