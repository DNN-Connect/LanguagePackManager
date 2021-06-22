namespace Connect.LanguagePackManager.Core.Common
{
    using System.IO;
    using System.Text;
    using System.Xml;

    public static class XmlExtensions
    {
        public static XmlNode AddChildElement(this XmlNode parent, string elementName)
        {
            var node = parent.OwnerDocument.CreateElement(elementName);
            parent.AppendChild(node);
            return node;
        }

        public static XmlNode AddChildElement(this XmlNode parent, string elementName, string value)
        {
            var node = parent.OwnerDocument.CreateElement(elementName);
            parent.AppendChild(node);
            node.InnerText = value;
            return node;
        }

        public static XmlNode AddChildElementIfNotNull(this XmlNode parent, string elementName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var node = parent.OwnerDocument.CreateElement(elementName);
                parent.AppendChild(node);
                node.InnerText = value;
                return node;
            }
            else { return null; }
        }

        public static XmlNode SetChildElement(this XmlNode parent, string elementName)
        {
            var node = parent.SelectSingleNode(elementName);
            if (node == null)
            {
                node = parent.OwnerDocument.CreateElement(elementName);
                parent.AppendChild(node);
            }
            return node;
        }

        public static XmlNode SetChildElement(this XmlNode parent, string elementName, string value)
        {
            var node = parent.SelectSingleNode(elementName);
            if (node == null)
            {
                node = parent.OwnerDocument.CreateElement(elementName);
                parent.AppendChild(node);
            }
            node.InnerText = value;
            return node;
        }

        public static XmlNode AddAttribute(this XmlNode parent, string attributeName, string value)
        {
            var att = parent.OwnerDocument.CreateAttribute(attributeName);
            att.Value = value;
            parent.Attributes.Append(att);
            return parent;
        }

        public static XmlNode SetAttribute(this XmlNode parent, string attributeName, string value)
        {
            var att = parent.Attributes[attributeName];
            if (att == null)
            {
                att = parent.OwnerDocument.CreateAttribute(attributeName);
                parent.Attributes.Append(att);
            }
            att.Value = value;
            return parent;
        }

        public static byte[] XmlToFormattedByteArray(this XmlDocument doc)
        {
            using (var w = new MemoryStream())
            {
                using (var xw = new XmlTextWriter(w, Encoding.UTF8))
                {
                    xw.Formatting = Formatting.Indented;
                    doc.WriteContentTo(xw);
                }

                return w.ToArray();
            }
        }
    }
}
