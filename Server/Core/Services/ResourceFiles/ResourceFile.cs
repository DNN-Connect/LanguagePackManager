using System;
using System.Collections.Generic;
using System.Xml;

namespace Connect.LanguagePackManager.Core.Services.ResourceFiles
{
    public class ResourceFile : XmlDocument
    {
        public Dictionary<string, string> Resources { get; set; } = new Dictionary<string, string>();

        public ResourceFile(string filePath)
        {
            this.Load(filePath);
            foreach (XmlNode x in this.DocumentElement.SelectNodes("/root/data"))
            {
                try
                {
                    string key = x.Attributes["name"].InnerText;
                    string value = x.SelectSingleNode("value").InnerXml;
                    Resources[key] = value;
                }
                catch (Exception ex)
                {
                    // ignore if we can't read the node
                }
            }

        }
    }
}
