using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Connect.LanguagePackManager.Core.Services.Github
{
    public class GithubService
    {
        public static List<GithubRelease> GetReleases(string org, string repo)
        {
            return GetJsonObject<List<GithubRelease>>("repos/{org}/{repo}/releases?per_page=99");
        }

        private static T GetJsonObject<T>(string relativeUrl)
        {
            var request = (HttpWebRequest)WebRequest.Create($"https://api.github.com/{relativeUrl}");
            request.Headers.Set(HttpRequestHeader.UserAgent, "Connect.LanguagePackManager");
            request.Headers.Add(HttpRequestHeader.Accept, "application/vnd.github.v3+json");
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
                    client.DownloadFile(fileUrl, destinationFile);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
