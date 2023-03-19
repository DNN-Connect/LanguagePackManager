namespace Connect.LanguagePackManager.Core.Services.Github
{
  using System.Collections.Generic;
  using System.Runtime.Serialization;

  [DataContract]
  public class GithubTree
  {
    [DataMember(Name = "sha")]
    public string Sha { get; set; }

    [DataMember(Name = "url")]
    public string Url { get; set; }

    [DataMember(Name = "tree")]
    public List<GithubFile> Files { get; set; }

    [DataMember(Name = "truncated")]
    public bool Truncated { get; set; }
  }
}
