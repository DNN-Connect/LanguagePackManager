namespace Connect.LanguagePackManager.Core.Services.Github
{
  using System.Runtime.Serialization;

  [DataContract]
  public class GithubBlob
  {
    [DataMember(Name = "content")]
    public string Content { get; set; }

    [DataMember(Name = "encoding")]
    public string Encoding { get; set; }

    [DataMember(Name = "sha")]
    public string Sha { get; set; }

    [DataMember(Name = "size")]
    public int Size { get; set; }
  }
}
