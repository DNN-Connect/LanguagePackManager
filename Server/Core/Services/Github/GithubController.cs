using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Helpers;
using Connect.LanguagePackManager.Core.Models.PackageLinks;
using Connect.LanguagePackManager.Core.Repositories;
using Connect.LanguagePackManager.Core.Services.Packages;
using DotNetNuke.UI.UserControls;
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
    }

    public static void CheckResourcesRepo(PackageLink package)
    {
      var repo = GithubService.GetRepo(package.OrgName, package.RepoName);
      var defaultBranch = repo.DefaultBranch;
      var zipUrl = $"https://github.com/{package.OrgName}/{package.RepoName}/archive/refs/heads/{defaultBranch}.zip";

      using (var client = new System.Net.WebClient())
      {
        using (var strm = client.OpenRead(zipUrl))
        {
          var unzipResult = ZipHelper.Unzip(strm, "", true);
          var manifest = new LanguagePackManifest(unzipResult, package.ModuleId);
          Globals.SaveObject($@"D:\tmp{package.RepoName}.json", unzipResult);
          manifest.Process(true, -1);

          try
          {
            //Directory.Delete(unzipResult.UnzipDirectory, true);
          }
          catch
          {
          }
        }
      }

      //var unzipResult = ZipHelper.Unzip(this.ControllerContext.HttpContext.Request.Files[0].InputStream, "", true);
      //var manifest = new LanguagePackManifest(unzipResult, ModuleContext.ModuleId);
      //manifest.Process(generic, User.UserID);

      //try
      //{
      //  Directory.Delete(unzipResult.UnzipDirectory, true);
      //}
      //catch
      //{
      //}


      //var lastCommit = GithubService.GetLastCommit(package.OrgName, package.RepoName);
      //if (lastCommit != null && lastCommit.Details != null && lastCommit.Details.Committer != null)
      //{
      //  var lastCommitDate = lastCommit.Details.Committer.Date;
      //  if (package.LastChecked == null || lastCommitDate > package.LastChecked)
      //  {
      //    var tree = GithubService.GetFileTree(package.OrgName, package.RepoName, lastCommit.Sha);
      //    if (tree != null && tree.Files != null)
      //    {
      //      var allFiles = tree.Files.Where(f => f.Type == "blob").ToList();
      //      var dnnFile = allFiles.Where(f => f.Path.ToLowerInvariant().EndsWith(".dnn")).FirstOrDefault();
      //      if (dnnFile != null)
      //      {
      //        var manifest = new LanguagePackManifest(tree, dnnFile, package.ModuleId);
      //        manifest.Process(true, -1);
      //      }
      //    }
      //  }
      //}
      //package.LastChecked = DateTime.Now;
      //PackageLinkRepository.Instance.UpdatePackageLink(package.GetPackageLinkBase(), -1);
    }

    public static string GetGithubPackage(string url)
    {
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
