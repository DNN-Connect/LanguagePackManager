using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Repositories;
using DotNetNuke.Web.Api;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Connect.LanguagePackManager.Core.Api
{
  public class PacksController : DnnApiController
  {
    public class ResxResources
    {
      public IDictionary<string, IDictionary<string, string>> Resources { get; set; }
    }

    [HttpGet]
    [AllowAnonymous]
    public HttpResponseMessage Resources(string packageName, string version, string locale)
    {
      version = version.ParseVersion().ToNormalizedFormat();
      var packageVersion = PackageVersionRepository.Instance.GetPackageVersion(PortalSettings.PortalId, packageName, version);
      var modInfo = DotNetNuke.Entities.Modules.ModuleController.Instance.GetModule(packageVersion.ModuleId, -1, true);
      var settings = ModuleSettings.GetSettings(modInfo);
      var localeChain = LocaleRepository.Instance.GetLocaleChain(locale).ToList();
      if (localeChain.Count == 0)
      {
        return Request.CreateResponse(HttpStatusCode.OK);
      }
      var genericLocaleId = localeChain[0].LocaleId;
      var specificLocaleId = -1;
      if (localeChain.Count == 2)
      {
        specificLocaleId = localeChain[1].LocaleId;
      }
      var pattern = $".{locale}.resx";
      var loc = new CultureInfo(locale);

      var res = new ResxResources()
      {
        Resources = new Dictionary<string, IDictionary<string, string>>()
      };

      var pack = Data.Sprocs.GetPack(packageVersion.PackageId, version, genericLocaleId, specificLocaleId);
      var fileList = pack.Select(p => p.FilePath).Distinct();
      if (fileList.Count() > 0)
      {
        foreach (var filePath in fileList)
        {
          var resxFile = new Dictionary<string, string>();
          var targetPath = filePath.ReplaceEnd(".resx", pattern);
          foreach (var entry in pack.Where(p => p.FilePath == filePath))
          {
            resxFile[entry.TextKey] = entry.TextValue;
          }
          res.Resources[targetPath] = resxFile;
        }
      }

      return Request.CreateResponse(HttpStatusCode.OK, res);
    }

    public class StatsDTO
    {
      public class PackageDTO
      {
        [JsonProperty("packageName")]
        public string PackageName { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
      }

      [JsonProperty("packages")]
      public PackageDTO[] Packages { get; set; }

      [JsonProperty("locales")]
      public string[] Locales { get; set; }
    }

    [HttpPost]
    [AllowAnonymous]
    public HttpResponseMessage Stats(StatsDTO data)
    {
      var packages = new DataTable();
      packages.Columns.Add("PackageName");
      packages.Columns.Add("Version");

      foreach (var p in data.Packages)
      {
        packages.Rows.Add(p.PackageName, p.Version.ParseVersion().ToNormalizedFormat());
      }

      var existingLocales = LocaleRepository.Instance.GetLocales();

      var locales = new DataTable();
      locales.Columns.Add("Version");

      foreach (var l in data.Locales)
      {
        if (existingLocales.FirstOrDefault(l1 => l1.Code == l) != null)
        {
          locales.Rows.Add(l);
        }
        else if (l.IndexOf("-") > 0 && existingLocales.FirstOrDefault(l1 => l1.Code == l.Substring(0, l.IndexOf("-"))) != null)
        {
          locales.Rows.Add(l.Substring(0, l.IndexOf("-")));
        }
      }

      var res = Data.Sprocs.GetStats(PortalSettings.PortalId, packages, locales);

      return Request.CreateResponse(HttpStatusCode.OK, res);
    }
  }
}
