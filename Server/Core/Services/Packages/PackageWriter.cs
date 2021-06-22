using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Models.PackageVersions;
using Connect.LanguagePackManager.Core.Repositories;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Connect.LanguagePackManager.Core.Services.Packages
{
    public class PackageWriter
    {
        public static string CreateResourcePack(int portalId, string packageName, string version, string locale, bool isFullPack)
        {
            string fileName = "";

            var packageVersion = PackageVersionRepository.Instance.GetPackageVersion(portalId, packageName, version);
            if (packageVersion == null) return "";

            var packageList = new List<PackageVersion>();
            packageList.Add(packageVersion);

            if (isFullPack)
            {
                foreach (var pv in PackageVersionRepository.Instance.GetChildPackageVersions(packageVersion.PackageVersionId))
                {
                    packageList.Add(pv);
                }
            }

            var localeChain = LocaleRepository.Instance.GetLocaleChain(locale).ToList();
            if (localeChain.Count == 0)
            {
                return "";
            }
            var genericLocaleId = localeChain[0].LocaleId;
            var specificLocaleId = -1;
            if (localeChain.Count == 2)
            {
                specificLocaleId = localeChain[1].LocaleId;
            }

            fileName = "ResourcePack." + CleanName(packageName);
            fileName += "." + version + "." + locale + ".zip";
            string packPath = Common.Globals.GetLpmFolder(portalId, "Cache") + @"\";

            // check for caching
            if (File.Exists(packPath + fileName))
            {
                var f = new FileInfo(packPath + fileName);
                var lastPackWriteTime = f.LastWriteTime;
                bool isCached = true;
                foreach (var pv in packageList)
                {
                    var lastEditTime = Data.Sprocs.GetLastEditTime(pv.PackageId, pv.Version, genericLocaleId, specificLocaleId);
                    if (lastEditTime == null || lastEditTime > lastPackWriteTime)
                    {
                        isCached = false;
                    }
                }

                if (isCached)
                {
                    return fileName;
                }
            }

            var pattern = $".{locale}.resx";
            var loc = new CultureInfo(locale);
            var manifest = new ManifestWriter(loc);

            using (var zipFile = File.Create(packPath + fileName))
            {
                using (var zipStrm = new ZipArchive(zipFile, ZipArchiveMode.Create))
                {

                    foreach (var package in packageList)
                    {
                        var pack = Data.Sprocs.GetPack(package.PackageId, version, genericLocaleId, specificLocaleId);
                        var fileList = pack.Select(p => p.FilePath).Distinct();

                        foreach (var filePath in fileList)
                        {
                            var targetPath = filePath.ReplaceEnd(".resx", pattern);
                            var resx = new ResxFile();
                            foreach (var entry in pack.Where(p => p.FilePath == filePath))
                            {
                                resx.Resources[entry.TextKey] = entry.TextValue;
                            }
                            resx.Recreate();
                            zipStrm.WriteFileToZip(targetPath, resx.XmlToFormattedByteArray());
                        }

                        manifest.AddPackage(package.PackageName, package.PackageName, "", fileList.ToList()); // todo friendlyName and version
                    }

                    manifest.Compile();
                    zipStrm.WriteFileToZip($"{packageName}.dnn", manifest.XmlToFormattedByteArray());
                }
            }

            return fileName;
        }

        public static string CleanName(string name)
        {
            return name.Replace(@"\", "_").Replace("/", "_");
        }
    }
}
