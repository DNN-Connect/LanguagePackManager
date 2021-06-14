using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Models.PackageLinks;
using Connect.LanguagePackManager.Core.Repositories;
using Connect.LanguagePackManager.Core.Services.Packages;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Connect.LanguagePackManager.Core.Services.Github
{
    public class GithubController
    {
        public static void CheckPackage(PackageLink package)
        {
            var baseVersion = string.IsNullOrEmpty(package.LastDownloadedVersion) ? "00.00.00" : package.LastDownloadedVersion;
            var githubVersions = GithubService.GetReleases(package.OrgName, package.RepoName)
                .Where(gp => gp.Draft == false
                            && gp.Prerelease == false
                            && (string.IsNullOrEmpty(package.LastDownloadedVersion) || baseVersion.IsSmallerThan(gp.TagName.ParseVersion().ToNormalizedFormat())))
                .OrderBy(p => p.Published);
            foreach (var githubVersion in githubVersions)
            {
                foreach (var download in githubVersion.Assets)
                {
                    var m = Regex.Match(download.Name, package.AssetRegex);
                    if (m.Success)
                    {
                        var result = GetGithubPackage(download.DownloadUrl);
                        if (!string.IsNullOrEmpty(result))
                        {
                            package.LastDownloadedVersion = githubVersion.TagName.ParseVersion().ToNormalizedFormat();
                            PackageLinkRepository.Instance.UpdatePackageLink(package.GetPackageLinkBase(), -1);
                            var reader = new PackageReader(package.PackageLinkId, result);
                            if (!reader.IsInError)
                            {
                                reader.Process(githubVersion.Published);
                            }
                        }
                    }
                }
            }
        }

        public static string GetGithubPackage(string url)
        {
            var fileToDownload = url.Substring(url.LastIndexOf('/') + 1);
            var fileToSave = Path.Combine(Globals.GetTempFolder(), fileToDownload);
            if (File.Exists(fileToSave)) return fileToSave;
            if (GithubService.DownloadFile(url, fileToSave))
            {
                return fileToSave;
            }
            return "";
        }
    }
}
