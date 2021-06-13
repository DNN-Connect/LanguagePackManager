using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Helpers;
using Connect.LanguagePackManager.Core.Models.Packages;
using Connect.LanguagePackManager.Core.Models.PackageVersions;
using Connect.LanguagePackManager.Core.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
    public class PackageReader
    {
        public int PackageLinkId { get; set; }
        private string WorkingDirectory { get; set; }
        private string[] DnnFiles { get; set; }
        public bool IsInError { get; set; } = false;
        public string ErrorMessage { get; set; } = "";
        public bool IsCore { get; set; } = false;
        public Manifest Manifest { get; set; } = null;
        private SortedList CoreResourceFiles { get; set; } = new SortedList();

        public PackageReader(int packageLinkId, string zipFilePath)
        {
            this.PackageLinkId = packageLinkId;

            // Create a temporary directory to unpack the package
            this.WorkingDirectory = Path.Combine(Globals.GetTempFolder(), @"LocalizationEditor\~tmp" + DateTime.Now.ToString("yyyyMMdd-hhmmss"));

            //Globals.CleanupTempDirs(homeDirectoryMapPath + "LocalizationEditor");

            // Unzip file contents
            ZipHelper.Unzip(zipFilePath, WorkingDirectory);

            // Find the DNN manifest file
            // Multiple manifests are allowed (.dnn and .dnn5 etc) so we look for the highest
            this.DnnFiles = Directory.GetFiles(WorkingDirectory, "*.dnn6");
            if (DnnFiles.Length == 0)
            {
                DnnFiles = Directory.GetFiles(WorkingDirectory, "*.dnn5");
            }

            if (DnnFiles.Length == 0)
            {
                DnnFiles = Directory.GetFiles(WorkingDirectory, "*.dnn");
            }

            if (DnnFiles.Length == 0)
            {
                if (Directory.GetFiles(WorkingDirectory, "Default.aspx").Length == 0)
                {
                    this.ErrorMessage = "No DNN Manifest file found, nor a core distribution";
                    this.IsInError = true;
                }
                else
                {
                    this.IsCore = true;
                }
            }
            else
            {
                this.Manifest = new Manifest(DnnFiles[0], this.WorkingDirectory);
            }
        }

        public void Process(DateTime releaseDate)
        {
            if (this.IsCore)
            {
                this.FindResourceFiles(new DirectoryInfo(WorkingDirectory), "");
                var version = Globals.GetAssemblyVersion(Path.Combine(WorkingDirectory, @"\bin\DotNetNuke.dll"));
                this.ProcessPackage(Globals.glbCoreName, "", Globals.glbCoreName, Globals.glbCoreFriendlyName, version, releaseDate, CoreResourceFiles);
            }
            else
            {
                foreach (var p in this.Manifest.Packages)
                {
                    this.ProcessPackage(p.PackageName, p.FolderName, p.PackageType, p.FriendlyName, p.PackageVersion, releaseDate, p.ResourceFiles);
                }
            }
            try
            {
                Directory.Delete(this.WorkingDirectory, true);
            }
            catch (Exception ex)
            {
            }
        }

        private void ProcessPackage(string packageName, string folderName, string packageType, string friendlyName, Version pVersion, DateTime releaseDate, SortedList resourceFiles)
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
                packageVersion.PackageVersionId = PackageVersionRepository.Instance.AddPackageVersion(packageVersion.GetPackageVersionBase()).PackageVersionId;
            }

            foreach (var resKey in resourceFiles.Keys)
            {
                this.ProcessResourceFile(packageVersion, package.LastVersion, (string)resKey, (string)resourceFiles[resKey]);
            }

            if (package.LastVersion.ParseVersion().CompareTo(pVersion) < 0)
            {
                package.LastVersion = version;
            }
            package.LastChecked = DateTime.Now;
            PackageRepository.Instance.UpdatePackage(package.GetPackageBase());
        }

        private void FindResourceFiles(DirectoryInfo dir, string basePath)
        {
            foreach (FileInfo f in dir.GetFiles("*.resx"))
            {
                var m = Regex.Match(f.Name, @"\.(\w{2,3}-\w\w)\.");
                if (!m.Success || m.Groups[1].Value.ToLower() == "en-us") // filter out all files that are not default locale
                {
                    string resKey = Path.Combine(basePath, f.Name);
                    this.CoreResourceFiles.Add(resKey, f);
                }
            }

            foreach (var d in dir.GetDirectories())
            {
                this.FindResourceFiles(d, basePath + d.Name + "/");
            }
        }

        private void ProcessResourceFile(PackageVersion packageVersion, string highestVersion, string fileKey, string filePath)
        {
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
                    var dbCurrentText = textsInDb.FirstOrDefault(t => t.TextKey == key && t.DeprecatedInVersion == null);
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
                    var dbCurrentText = textsInDb.FirstOrDefault(t => t.TextKey == key && t.DeprecatedInVersion == null);
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
                foreach (var dbText in textsInDb.Where(t => string.IsNullOrEmpty(t.DeprecatedInVersion)))
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
