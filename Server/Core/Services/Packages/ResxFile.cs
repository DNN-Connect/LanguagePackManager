using System.Collections.Generic;
using System.Xml;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
    public class ResxFile : XmlDocument
    {
        public string FileKey { get; set; }
        public Dictionary<string, string> Resources { get; set; } = new Dictionary<string, string>();

        public ResxFile(string fileKey, string loadXmlPath)
        {
            this.FileKey = fileKey;
            this.Load(loadXmlPath);


            foreach (XmlNode entry in this.SelectNodes("root/data"))
            {
                var key = entry.SelectSingleNode("@name").InnerText;
                var value = entry.SelectSingleNode("value").InnerText;
                this.Resources[key] = value;
            }
        }
    }
}
