using Connect.LanguagePackManager.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace Connect.LanguagePackManager.Core.Helpers
{
    public class UnzipResult
    {
        public class FoundFile
        {
            public string FilePath { get; set; }
            public string FilePathLowered { get; set; }
            public string HashedName { get; set; }
        }

        public string UnzipDirectory { get; set; } = Path.Combine(Globals.GetLpmFolder(-1, "Temp"), (Guid.NewGuid()).ToString());
        public SortedDictionary<string, FoundFile> ResourceFiles { get; set; } = new SortedDictionary<string, FoundFile>();
        public SortedDictionary<string, FoundFile> ZipFiles { get; set; } = new SortedDictionary<string, FoundFile>();
        public string ManifestFile { get; set; }
        public Version DnnVersion { get; set; }
        public string BasePath { get; set; }

        public UnzipResult(string basePath)
        {
            this.BasePath = basePath;
            Directory.CreateDirectory(UnzipDirectory);
        }

        public void AddResourceFile(string filePath, string hashedName)
        {
            var f = new FoundFile()
            {
                FilePath = filePath,
                FilePathLowered = filePath.ToLower(),
                HashedName = hashedName
            };
            this.ResourceFiles.Add(f.FilePathLowered, f);
        }

        public void AddZipFile(string filePath, string hashedName)
        {
            var f = new FoundFile()
            {
                FilePath = filePath,
                FilePathLowered = filePath.ToLower(),
                HashedName = hashedName
            };
            this.ZipFiles.Add(f.FilePathLowered, f);
        }

    }
}
