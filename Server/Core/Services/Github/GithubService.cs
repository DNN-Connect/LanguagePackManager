using Connect.LanguagePackManager.Core.Services.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Connect.LanguagePackManager.Core.Services.Github
{
  public class GithubService
  {
    public static List<GithubRelease> GetReleases(string org, string repo)
    {
      return GetJsonObject<List<GithubRelease>>($"repos/{org}/{repo}/releases?per_page=99");
    }

    public static GithubTree GetFileTree(string org, string repo, string treeSha)
    {
      return GetJsonObject<GithubTree>($"repos/{org}/{repo}/git/trees/{treeSha}?recursive=1");
    }

    public static GithubCommit GetLastCommit(string org, string repo)
    {
      return GetJsonObject<List<GithubCommit>>($"repos/{org}/{repo}/commits?per_page=1&page=1")
                .FirstOrDefault();
    }

    public static GithubRepo GetRepo(string org, string repo)
    {
      return GetJsonObject<GithubRepo>($"repos/{org}/{repo}");
    }

    private static T GetJsonObject<T>(string relativeUrl)
    {
      var request = (HttpWebRequest)WebRequest.Create($"https://api.github.com/{relativeUrl}");
      request.UserAgent = "Connect.LPM";
      request.Accept = "application/vnd.github.v3+json";
      using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
      {
        if (response.StatusCode == HttpStatusCode.OK)
        {
          var dataStream = response.GetResponseStream();
          var reader = new StreamReader(dataStream);
          var responseFromServer = reader.ReadToEnd();
          return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseFromServer);
        }
      }

      return default(T);
    }


    public static bool DownloadFile(string fileUrl, string destinationFile)
    {
      try
      {
        using (var client = new WebClient())
        {
          ServicePointManager.Expect100Continue = true;
          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
          client.Headers.Add("user-agent", "request");
          
          client.DownloadFile(fileUrl, destinationFile);
        }
      }
      catch (Exception ex)
      {
        return false;
      }

      return true;
    }

    public static string DownloadFile(string fileUrl)
    {
      try
      {
        using (var client = new WebClient())
        {
          ServicePointManager.Expect100Continue = true;
          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
          client.Headers.Add("user-agent", "request");

          var blobJsonBytes = client.DownloadData(fileUrl);
          var blobJson = Encoding.UTF8.GetString(blobJsonBytes, 0, blobJsonBytes.Length);
          var blob = Newtonsoft.Json.JsonConvert.DeserializeObject<GithubBlob>(blobJson);
          var blobBytes = Convert.FromBase64String(blob.Content);
          return Encoding.UTF8.GetString(blobBytes, 0, blobBytes.Length);
        }
      }
      catch (Exception ex)
      {
        return null;
      }
    }
  }
}
