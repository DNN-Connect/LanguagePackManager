using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Services.Packages;
using Connect.LanguagePackManager.Presentation.Common;
using System.IO;
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
    }
}
