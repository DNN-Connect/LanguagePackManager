namespace Connect.LanguagePackManager.Core.Services.Github
{
  using System.Runtime.Serialization;

  [DataContract]
  public class GithubFile
  {
    [DataMember(Name = "path")]
    public string Path { get; set; }

    [DataMember(Name = "mode")]
    public string Mode { get; set; }

    [DataMember(Name = "type")]
    public string Type { get; set; }

    [DataMember(Name = "size")]
    public int Size { get; set; }

    [DataMember(Name = "sha")]
    public string Sha { get; set; }

    [DataMember(Name = "url")]
    public string Url { get; set; }
  }
}
