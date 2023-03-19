using Connect.LanguagePackManager.Core.Common;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Connect.LanguagePackManager.Core.Helpers
{
  public class ZipHelper
  {
    public static UnzipResult Unzip(Stream fileStream, string packageBasePath, bool allowTranslations)
    {
      var result = new UnzipResult(packageBasePath);
      using (var objZipInputStream = new ZipArchive(fileStream, ZipArchiveMode.Read))
      {
        foreach (var entry in objZipInputStream.Entries)
        {
          var fullName = entry.FullName.Trim('/').ToLowerInvariant();
          var ext = Path.GetExtension(fullName);
          switch (ext)
          {
            case ".dnn":
              entry.ExtractToFile(Path.Combine(result.UnzipDirectory, entry.Name), true);
              result.ManifestFile = entry.Name;
              result.ManifestFilePath = Path.GetDirectoryName(entry.FullName);
              break;
            case ".dll":
              if (entry.Name.ToLowerInvariant() == "dotnetnuke.dll")
              {
                entry.ExtractToFile(Path.Combine(result.UnzipDirectory, entry.Name), true);
                result.DnnVersion = Globals.GetAssemblyVersion(Path.Combine(result.UnzipDirectory, entry.Name));
                try
                {
                  File.Delete(Path.Combine(result.UnzipDirectory, entry.Name));
                }
                catch
                {
                }
              }
              break;
            case ".resx":
              var fileName = fullName.ToMD5Hash();
              var m = Regex.Match(entry.Name, @"\.(\w{2,3}-\w\w)\.");
              if (allowTranslations || !m.Success || m.Groups[1].Value.ToLower() == "en-us") // filter out all files that are not default locale
              {
                entry.ExtractToFile(Path.Combine(result.UnzipDirectory, fileName));
                result.AddResourceFile(fullName, fileName);
              }
              break;
            case ".resources":
            case ".zip":
              var zipName = fullName.ToMD5Hash();
              entry.ExtractToFile(Path.Combine(result.UnzipDirectory, zipName));
              result.AddZipFile(fullName, zipName);
              break;
          }
        }
      }
      if (!string.IsNullOrEmpty(result.ManifestFilePath))
      {
        var l = result.ManifestFilePath.Length + 1;
        foreach (var resx in result.ResourceFiles.Values)
        {
          resx.FilePath = resx.FilePath.Remove(0, l);
          resx.FilePathLowered = resx.FilePathLowered.Remove(0, l);
        }
      }
      return result;
    }

    public static UnzipResult Unzip(string filePath, string packageBasePath, bool allowTranslations)
    {
      using (var fileStrm = File.Open(filePath, FileMode.Open, FileAccess.Read))
      {
        return Unzip(fileStrm, packageBasePath, allowTranslations);
      }
    }
  }
}