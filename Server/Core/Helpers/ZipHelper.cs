using Connect.LanguagePackManager.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect.LanguagePackManager.Core.Helpers
{
    public class ZipHelper
    {
        public static void Unzip(Stream fileStream, string tempDirectory)
        {
            using (var objZipInputStream = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
                objZipInputStream.ExtractToDirectory(tempDirectory);
            }
        }

        public static void Unzip(string filePath, string tempDirectory)
        {
            using (var fileStrm = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                Unzip(fileStrm, tempDirectory);
            }
        }
    }
}