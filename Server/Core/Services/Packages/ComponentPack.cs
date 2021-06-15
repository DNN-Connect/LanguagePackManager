using Connect.LanguagePackManager.Core.Models.Packages;
using System.Collections.Generic;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
    public class ComponentPack
    {
        public Package Package { get; set; }
        public string Version { get; set; }
        public string Locale { get; set; }
        public List<ResxFile> ResourceFiles { get; set; } = new List<ResxFile>();
    }
}
