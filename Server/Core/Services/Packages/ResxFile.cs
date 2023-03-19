using Connect.LanguagePackManager.Core.Common;
using System.Collections.Generic;
using System.Xml;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
  public class ResxFile : XmlDocument
  {
    public string FileKey { get; set; }
    public Dictionary<string, string> Resources { get; set; } = new Dictionary<string, string>();

    public ResxFile(string fileKey, string xmlContents)
    {

      this.FileKey = fileKey;
      this.LoadXml(xmlContents);

      foreach (XmlNode entry in this.SelectNodes("root/data"))
      {
        var key = entry.SelectSingleNode("@name").InnerText;
        var value = entry.SelectSingleNode("value").InnerText;
        this.Resources[key] = value;
      }
    }

    public ResxFile()
    {
    }

    public void Recreate()
    {
      this.Load(DotNetNuke.Common.Globals.ApplicationMapPath + @"\DesktopModules\MVC\Connect\LanguagePackManager\App_LocalResources\Template.resx");
      foreach (var key in this.Resources.Keys)
      {
        this.AddResourceText(key, this.Resources[key]);
      }
    }

    private void AddResourceText(string textKey, string textValue)
    {
      var newNode = this.DocumentElement.AddChildElement("data");
      newNode.AddAttribute("name", textKey);
      newNode.AddAttribute("xml:space", "preserve");
      if (string.IsNullOrEmpty(textValue))
      {
        newNode.AddChildElement("value", "");
      }
      else
      {
        newNode.AddChildElement("value", textValue);
      }
    }
  }
}
