using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Helpers;
using Connect.LanguagePackManager.Core.Models.PackageLinks;
using Connect.LanguagePackManager.Core.Repositories;
using Connect.LanguagePackManager.Core.Services.Packages;
using DotNetNuke.Instrumentation;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Connect.LanguagePackManager.Core.Services.Github
{
  public class GithubController
  {
    private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(GithubController));

    public static void CheckPackage(PackageLink package)
    {
      Logger.Info($"Checking package repo {package.Name}");
      var baseVersion = string.IsNullOrEmpty(package.LastDownloadedVersion) ? "00.00.00" : package.LastDownloadedVersion;
      var gh1 = GithubService.GetReleases(package.OrgName, package.RepoName);
      var gh2 = gh1.Where(gp => gp.Draft == false);
      var gh3 = gh2.Where(gp => gp.Prerelease == false);
      var gh4 = gh3.Where(gp => (string.IsNullOrEmpty(package.LastDownloadedVersion) || baseVersion.IsSmallerThan(gp.TagName.ParseVersion().ToNormalizedFormat())));
      var githubVersions = gh4.OrderBy(p => p.Published);
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
              var reader = new PackageReader(package.PackageLinkId, -1, result);
              if (!reader.IsInError)
              {
                reader.Process(githubVersion.Published);
              }
              package.LastDownloadedVersion = githubVersion.TagName.ParseVersion().ToNormalizedFormat();
            }
          }
        }
      }
      package.LastChecked = DateTime.Now;
      PackageLinkRepository.Instance.UpdatePackageLink(package.GetPackageLinkBase(), -1);
      Logger.Info($"Finished checking package repo {package.Name}");
    }

    public static int CheckResourcesRepo(PackageLink package)
    {
      Logger.Info($"Checking resources repo {package.Name}");
      var downloadedReleases = 0;
      var lastCommit = GithubService.GetLastCommit(package.OrgName, package.RepoName);
      if (lastCommit != null && lastCommit.Details != null && lastCommit.Details.Committer != null)
      {
        var baseVersion = string.IsNullOrEmpty(package.LastDownloadedVersion) ? "00.00.00" : package.LastDownloadedVersion;
        var githubVersions = GithubService.GetReleases(package.OrgName, package.RepoName)
            .Where(gp => gp.Draft == false && gp.Prerelease == false)
            .OrderBy(p => p.Published);
        if (githubVersions.Any())
        {
          // this repo posts releases
          foreach (var githubVersion in githubVersions.Where(gp => string.IsNullOrEmpty(package.LastDownloadedVersion) || baseVersion.IsSmallerThan(gp.TagName.ParseVersion().ToNormalizedFormat())))
          {
            foreach (var download in githubVersion.Assets)
            {
              var m = Regex.Match(download.Name, package.AssetRegex);
              if (m.Success)
              {
                var result = GetGithubPackage(download.DownloadUrl);
                if (!string.IsNullOrEmpty(result))
                {
                  var unzipResult = ZipHelper.Unzip(result, "", true);
                  var manifest = new LanguagePackManifest(unzipResult, package.ModuleId);
                  manifest.Process(true, -1);
                  downloadedReleases++;

                  try
                  {
                    Directory.Delete(unzipResult.UnzipDirectory, true);
                  }
                  catch
                  {
                  }

                  package.LastDownloadedVersion = githubVersion.TagName.ParseVersion().ToNormalizedFormat();
                }
              }
            }
          }
        }
        else
        {
          // this repo doesn't post releases but just keeps an updated list of resources
          var repo = GithubService.GetRepo(package.OrgName, package.RepoName);
          var defaultBranch = repo.DefaultBranch;
          var zipUrl = $"https://github.com/{package.OrgName}/{package.RepoName}/archive/refs/heads/{defaultBranch}.zip";

          using (var client = new System.Net.WebClient())
          {
            using (var strm = client.OpenRead(zipUrl))
            {
              var unzipResult = ZipHelper.Unzip(strm, "", true);
              var manifest = new LanguagePackManifest(unzipResult, package.ModuleId);
              manifest.Process(true, -1);
              downloadedReleases++;

              try
              {
                Directory.Delete(unzipResult.UnzipDirectory, true);
              }
              catch
              {
              }
            }
          }
        }
      }
      package.LastChecked = DateTime.Now;
      PackageLinkRepository.Instance.UpdatePackageLink(package.GetPackageLinkBase(), -1);
      Logger.Info($"Finished checking resources repo {package.Name}");
      return downloadedReleases;
    }

    public static string GetGithubPackage(string url)
    {
      Logger.Info($"Getting release file {url}");
      var fileToDownload = url.Substring(url.LastIndexOf('/') + 1);
      var fileToSave = Path.Combine(Globals.GetLpmFolder(-1, "Temp"), fileToDownload);
      if (File.Exists(fileToSave)) return fileToSave;
      if (GithubService.DownloadFile(url, fileToSave))
      {
        return fileToSave;
      }
      return "";
    }
  }
}
