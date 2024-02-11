using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Helpers;
using Connect.LanguagePackManager.Core.Models.Packages;
using Connect.LanguagePackManager.Core.Models.PackageVersions;
using Connect.LanguagePackManager.Core.Repositories;
using DotNetNuke.Instrumentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
  public class PackageReader
  {
    private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(PackageReader));

    public int PackageLinkId { get; set; }
    public int ParentPackageVersionId { get; set; }
    private UnzipResult UnzipResult { get; set; }
    public bool IsInError { get; set; } = false;
    public string ErrorMessage { get; set; } = "";
    public bool IsCore { get; set; } = false;
    public Manifest Manifest { get; set; } = null;

    public PackageReader(int packageLinkId, int parentPackageVersionId, string zipFilePath)
    {
      Logger.Info($"Reading package {zipFilePath}");
      this.PackageLinkId = packageLinkId;
      this.ParentPackageVersionId = parentPackageVersionId;

      this.UnzipResult = ZipHelper.Unzip(zipFilePath, "", false);

      if (string.IsNullOrEmpty(UnzipResult.ManifestFile))
      {
        if (this.UnzipResult.DnnVersion == new System.Version(0, 0, 0))
        {
          this.ErrorMessage = "No DNN Manifest file found, nor a core distribution";
          Logger.Info("No DNN Manifest file found, nor a core distribution");
          this.IsInError = true;
        }
        else
        {
          Logger.Info("This is a core distribution");
          this.IsCore = true;
        }
      }
      else
      {
        Logger.Info("Regular DNN package");
        try
        {
          this.Manifest = new Manifest(this.UnzipResult);
        }
        catch (Exception ex)
        {
          Logger.Error("Cannot parse manifest of package");
          Logger.Error(ex);
          this.IsInError= true;
          this.ErrorMessage = "Cannot parse manifest of package";
        }
      }
    }

    public void Process(DateTime releaseDate)
    {
      if (this.IsCore)
      {
        var coreId = this.ProcessPackage(Globals.glbCoreName, "", Globals.glbCoreName, Globals.glbCoreFriendlyName, this.UnzipResult.DnnVersion, releaseDate);
        foreach (var zipFile in this.UnzipResult.ZipFiles.Values)
        {
          if (zipFile.FilePathLowered.StartsWith("install"))
          {
            try
            {
              var rdr = new PackageReader(this.PackageLinkId, coreId, Path.Combine(this.UnzipResult.UnzipDirectory, zipFile.HashedName));
              if (!rdr.IsInError)
              {
                rdr.Process(releaseDate);
              }
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      else
      {
        foreach (var p in this.Manifest.Packages)
        {
          this.ProcessPackage(p.PackageName, p.FolderName, p.PackageType, p.FriendlyName, p.PackageVersion, releaseDate, p.ResourcesFile);
        }
      }
      try
      {
        Directory.Delete(this.UnzipResult.UnzipDirectory, true);
      }
      catch (Exception ex)
      {
      }
    }

    private int ProcessPackage(string packageName, string folderName, string packageType, string friendlyName, Version pVersion, DateTime releaseDate, UnzipResult extraFiles = null)
    {
      var version = pVersion.ToNormalizedFormat();
      var package = PackageRepository.Instance.FindPackage(this.PackageLinkId, packageName);
      if (package == null)
      {
        package = new Package()
        {
          FriendlyName = friendlyName,
          InstallPath = folderName,
          LastChecked = DateTime.Now,
          LastVersion = "00.00.00",
          LinkId = PackageLinkId,
          PackageName = packageName,
          PackageType = packageType
        };
        package.PackageId = PackageRepository.Instance.AddPackage(package.GetPackageBase()).PackageId;
      }

      var packageVersion = PackageVersionRepository.Instance.GetPackageVersionsByPackage(package.PackageId).FirstOrDefault(pv => pv.Version == version);
      if (packageVersion == null)
      {
        packageVersion = new PackageVersion()
        {
          PackageId = package.PackageId,
          Version = version,
          ReleaseDate = releaseDate,
          Downloaded = DateTime.Now
        };
        if (this.ParentPackageVersionId != -1)
        {
          packageVersion.ContainedInPackageVersionId = this.ParentPackageVersionId;
        }
        packageVersion.PackageVersionId = PackageVersionRepository.Instance.AddPackageVersion(packageVersion.GetPackageVersionBase()).PackageVersionId;
      }

      foreach (var resFile in this.UnzipResult.ResourceFiles.Values)
      {
        this.ProcessResourceFile(packageVersion, package.LastVersion, Globals.DnnPathCombine(this.UnzipResult.BasePath, resFile.FilePathLowered), Path.Combine(this.UnzipResult.UnzipDirectory, resFile.HashedName));
      }
      if (extraFiles != null)
      {
        foreach (var resFile in extraFiles.ResourceFiles.Values)
        {
          this.ProcessResourceFile(packageVersion, package.LastVersion, Globals.DnnPathCombine(extraFiles.BasePath, resFile.FilePathLowered), Path.Combine(extraFiles.UnzipDirectory, resFile.HashedName));
        }
      }

      if (package.LastVersion.ParseVersion().CompareTo(pVersion) < 0)
      {
        package.LastVersion = version;
      }
      package.LastChecked = DateTime.Now;
      PackageRepository.Instance.UpdatePackage(package.GetPackageBase());

      return packageVersion.PackageVersionId;
    }

    private void ProcessResourceFile(PackageVersion packageVersion, string highestVersion, string fileKey, string filePath)
    {
      fileKey = fileKey.Trim('/');
      var processingNewPackage = packageVersion.Version.IsBiggerThan(highestVersion);

      var resFile = ResourceFileRepository.Instance.GetResourceFilesByPackage(packageVersion.PackageId).FirstOrDefault(r => r.FilePath == fileKey);
      if (resFile == null)
      {
        resFile = new Models.ResourceFiles.ResourceFile()
        {
          FilePath = fileKey,
          PackageId = packageVersion.PackageId
        };
        resFile = ResourceFileRepository.Instance.AddResourceFile(resFile);
      }
      var textsInDb = TextRepository.Instance.GetTextsByResourceFile(resFile.ResourceFileId);

      var foundTexts = new Dictionary<string, string>();
      try
      {
        foundTexts = new ResourceFiles.ResourceFile(filePath).Resources;
      }
      catch (Exception ex)
      {
        // not sure what to do if we can't read the file
      }

      foreach (var key in foundTexts.Keys)
      {
        var value = foundTexts[key];
        if (processingNewPackage)
        {
          var dbCurrentText = textsInDb.FirstOrDefault(t => t.TextKey == key && t.DeprecatedInVersion == "99.99.99");
          if (dbCurrentText == null)
          {
            // we never had this key in the DB
            TextRepository.Instance.AddText(packageVersion.PackageVersionId, resFile.ResourceFileId, key, value);
          }
          else if (dbCurrentText.OriginalValue != value)
          {
            dbCurrentText.DeprecatedInVersionId = packageVersion.PackageVersionId;
            TextRepository.Instance.UpdateText(dbCurrentText.GetTextBase());
            TextRepository.Instance.AddText(packageVersion.PackageVersionId, resFile.ResourceFileId, key, value);
          }
        }
        else
        {
          // this gets tricky as we need to work out how this fits in as it's an older version we're uploading
          var dbCurrentText = textsInDb.FirstOrDefault(t => t.TextKey == key && t.DeprecatedInVersion == "99.99.99");
          if (dbCurrentText == null)
          {
            // we never had this key in the DB but it didn't appear in the last version
            var textBase = TextRepository.Instance.AddText(packageVersion.PackageVersionId, resFile.ResourceFileId, key, value);
            var nextVersion = PackageVersionRepository.Instance.GetNextVersion(packageVersion.PackageId, packageVersion.Version);
            if (nextVersion != null)
            {
              textBase.DeprecatedInVersionId = nextVersion.PackageVersionId;
              TextRepository.Instance.UpdateText(textBase);
            }
          }
          else if (dbCurrentText.CoversVersion(packageVersion.Version))
          {
            if (dbCurrentText.OriginalValue == value)
            {
              // we're good. Just uploading an older version but with the same key as an even older version
            }
            else
            {
              // WTF. We had a different value in an intermediate version. This should not happen
            }
          }
          else
          {
            var dbStraddlingText = textsInDb.FirstOrDefault(t => t.TextKey == key && t.CoversVersion(packageVersion.Version));
            if (dbStraddlingText == null)
            {
              if (dbCurrentText.OriginalValue == value)
              {
                // pull crt text forward
                dbCurrentText.PackageVersionId = packageVersion.PackageVersionId;
                TextRepository.Instance.UpdateText(dbCurrentText.GetTextBase());
              }
              else
              {
                // we had a different value in the past
                var textBase = TextRepository.Instance.AddText(packageVersion.PackageVersionId, resFile.ResourceFileId, key, value);
                var nextVersion = PackageVersionRepository.Instance.GetNextVersion(packageVersion.PackageId, packageVersion.Version);
                if (nextVersion != null)
                {
                  textBase.DeprecatedInVersionId = nextVersion.PackageVersionId;
                  TextRepository.Instance.UpdateText(textBase);
                }
              }
            }
            else
            {
              if (dbStraddlingText.OriginalValue != value)
              {
                // WTF. Shouldn't happen.
              }
            }
          }
        }
      }

      // Now we should check if we need to deprecate any texts. Ignore if we're uploading an older package
      if (processingNewPackage)
      {
        foreach (var dbText in textsInDb.Where(t => t.DeprecatedInVersion == "99.99.99"))
        {
          if (!foundTexts.Keys.Contains(dbText.TextKey))
          {
            dbText.DeprecatedInVersionId = packageVersion.PackageVersionId;
            TextRepository.Instance.UpdateText(dbText.GetTextBase());
          }
        }
      }

    }
  }
}
