using Connect.LanguagePackManager.Core.Common;
using DotNetNuke.Instrumentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Connect.LanguagePackManager.Core.Services.Github
{
  public class GithubService
  {
    private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(GithubService));

    public static List<GithubRelease> GetReleases(string org, string repo)
    {
      return GetJsonObject<List<GithubRelease>>($"repos/{org}/{repo}/releases?per_page=99");
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
      try
      {
        Logger.Info($"Requesting {request.RequestUri}");
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
          if (response.StatusCode == HttpStatusCode.OK)
          {
            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            var responseFromServer = reader.ReadToEnd();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseFromServer);
          }
          else
          {
            Logger.Error($"Error requesting {request.RequestUri}: {response.StatusCode}");
          }
        }
      }
      catch (WebException wex)
      {
        if (wex.Response != null && wex.Response is HttpWebResponse)
        {
          var response = (HttpWebResponse)wex.Response;
          var body = response.GetResponseBody();
          Logger.Error($"Error requesting {request.RequestUri}: {response.StatusCode}");
          Logger.Error(wex);
          Logger.Error(body);
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

          Logger.Info($"Retrieving {fileUrl} and saving to {destinationFile}");
          client.DownloadFile(fileUrl, destinationFile);
        }
      }
      catch (Exception ex)
      {
        Logger.Error(ex);
        return false;
      }

      return true;
    }
  }
}
