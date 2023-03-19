namespace Connect.LanguagePackManager.Core.Services.Github
{
  using System.Runtime.Serialization;

  [DataContract]
  public class GithubRepo
  {
    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "full_name")]
    public string FullName { get; set; }

    [DataMember(Name = "description")]
    public string Description { get; set; }

    [DataMember(Name = "html_url")]
    public string HtmlUrl { get; set; }

    [DataMember(Name = "clone_url")]
    public string CloneUrl { get; set; }

    [DataMember(Name = "git_url")]
    public string GitUrl { get; set; }

    [DataMember(Name = "ssh_url")]
    public string SshUrl { get; set; }

    [DataMember(Name = "svn_url")]
    public string SvnUrl { get; set; }

    [DataMember(Name = "homepage")]
    public string Homepage { get; set; }

    [DataMember(Name = "language")]
    public string Language { get; set; }

    [DataMember(Name = "forks_count")]
    public int ForksCount { get; set; }

    [DataMember(Name = "stargazers_count")]
    public int StargazersCount { get; set; }

    [DataMember(Name = "watchers_count")]
    public int WatchersCount { get; set; }

    [DataMember(Name = "size")]
    public int Size { get; set; }

    [DataMember(Name = "default_branch")]
    public string DefaultBranch { get; set; }

    [DataMember(Name = "open_issues_count")]
    public int OpenIssuesCount { get; set; }

    [DataMember(Name = "has_issues")]
    public bool HasIssues { get; set; }

    [DataMember(Name = "has_wiki")]
    public bool HasWiki { get; set; }

    [DataMember(Name = "has_pages")]
    public bool HasPages { get; set; }

    [DataMember(Name = "has_downloads")]
    public bool HasDownloads { get; set; }

    [DataMember(Name = "pushed_at")]
    public string PushedAt { get; set; }

    [DataMember(Name = "created_at")]
    public string CreatedAt { get; set; }

    [DataMember(Name = "updated_at")]
    public string UpdatedAt { get; set; }

  }
}
