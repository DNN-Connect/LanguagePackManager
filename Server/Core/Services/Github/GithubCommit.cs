namespace Connect.LanguagePackManager.Core.Services.Github
{
  using System;
  using System.Runtime.Serialization;

  [DataContract]
  public class GithubCommit
  {
    [DataMember(Name = "sha")]
    public string Sha { get; set; }

    [DataMember(Name = "commit")]
    public GithubCommitCommitDetails Details { get; set; }
  }

  [DataContract]
  public class GithubCommitCommitDetails
  {
    [DataMember(Name = "author")]
    public GithubCommitPerson Author { get; set; }

    [DataMember(Name = "committer")]
    public GithubCommitPerson Committer { get; set; }
  }

  [DataContract]
  public class GithubCommitPerson
  {
    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "email")]
    public string Email { get; set; }

    [DataMember(Name = "date")]
    public DateTime Date { get; set; }
  }

}
