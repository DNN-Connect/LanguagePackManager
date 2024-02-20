using System;

namespace Connect.LanguagePackManager.Core.Models
{
  public class GetStatDTO
  {
    public string PackageName { get; set; }
    public string Version { get; set; }
    public string Code { get; set; }
    public int TotalTexts { get; set; }
    public int NrTexts { get; set; }
    public DateTime LastChange { get; set; }
  }
}
