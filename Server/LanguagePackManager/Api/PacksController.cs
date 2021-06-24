using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Repositories;
using Connect.LanguagePackManager.Core.Services.Packages;
using Connect.LanguagePackManager.Presentation.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Connect.LanguagePackManager.Presentation.Api
{
    public class PacksController : LanguagePackManagerApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string packageName, string version, string locale, bool full = true)
        {
            version = version.ParseVersion().ToNormalizedFormat();
            var packPath = PackageWriter.CreateResourcePack(PortalSettings.PortalId, packageName, version, locale, full);
            if (string.IsNullOrEmpty(packPath))
            {
                return ServiceError("Something went wrong");
            }
            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream mem = new MemoryStream();
            using (var fileStream = File.OpenRead(Path.Combine(Globals.GetLpmFolder(PortalSettings.PortalId, "Cache"), packPath)))
            {
                fileStream.CopyTo(mem);
            }
            mem.Seek(0, SeekOrigin.Begin);
            res.Content = new StreamContent(mem);
            res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
            res.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            res.Content.Headers.ContentDisposition.FileName = Path.GetFileName(packPath);
            return res;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Versions(int id)
        {
            var pv = PackageVersionRepository.Instance.GetPackageVersionsByPackage(id);
            return Request.CreateResponse(HttpStatusCode.OK, pv);
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage TextStats(int id, string locale)
        {
            var knownGenericLocales = LocaleRepository.Instance.GetLocales().Where(l => l.Code.Length == 2).Select(l => l.Code).ToList();
            var netLocale = new CultureInfo(locale);
            if (!knownGenericLocales.Contains(netLocale.TwoLetterISOLanguageName))
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var loc = LocaleRepository.Instance.GetOrCreateLocale(netLocale.Name);
            var nrTexts = PackageVersionLocaleTextCountRepository.Instance.GetPackageVersionLocaleTextCounts(loc.LocaleId)
                .Where(t => t.PackageId == id)
                .ToList();
            if (nrTexts.Count == 0)
            {
                Core.Data.Sprocs.RefreshLocaleTextCount(loc.LocaleId);
                nrTexts = PackageVersionLocaleTextCountRepository.Instance.GetPackageVersionLocaleTextCounts(loc.LocaleId)
                    .Where(t => t.PackageId == id)
                    .ToList();
            }
            return Request.CreateResponse(HttpStatusCode.OK, nrTexts);
        }
    }
}
