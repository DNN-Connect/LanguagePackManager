using System.IO;
using System.Linq;
using System.Web.Mvc;
using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Data;
using Connect.LanguagePackManager.Core.Helpers;
using Connect.LanguagePackManager.Core.Repositories;
using Connect.LanguagePackManager.Core.Services.Packages;
using Connect.LanguagePackManager.Presentation.Common;

namespace Connect.LanguagePackManager.Presentation.Controllers
{
    public class PacksController : LanguagePackManagerMvcController
    {
        [HttpGet]
        [LpmAuthorize(SecurityLevel = SecurityAccessLevel.Translator)]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LpmAuthorize(SecurityLevel = SecurityAccessLevel.Translator)]
        public ActionResult Upload(object data)
        {
            var generic = this.Request.Form["GenericLanguage"].OnOffToBool();
            var unzipResult = ZipHelper.Unzip(this.ControllerContext.HttpContext.Request.Files[0].InputStream, "", true);
            var manifest = new LanguagePackManifest(unzipResult, ModuleContext.ModuleId);

            foreach (var p in manifest.Components)
            {
                var locale = generic ? p.Locale.Substring(0, 2) : p.Locale;
                var localeId = LocaleRepository.Instance.GetOrCreateLocale(locale).LocaleId;
                var dbResFiles = ResourceFileRepository.Instance.GetResourceFilesByPackage(p.Package.PackageId);
                foreach (var rf in p.ResourceFiles)
                {
                    var dbFile = dbResFiles.FirstOrDefault(f => f.FilePath == rf.FileKey);
                    if (dbFile != null)
                    {
                        var dbTexts = TextRepository.Instance.GetTextsByResourceFile(dbFile.ResourceFileId);
                        foreach (var tt in rf.Resources.Keys)
                        {
                            var dbT = dbTexts.FirstOrDefault(t => t.TextKey == tt && t.CoversVersion(p.Version));
                            if (dbT != null)
                            {
                                Sprocs.SetTranslation(dbT.TextId, localeId, rf.Resources[tt], User.UserID);
                            }
                        }
                    }
                }
            }

            try
            {
                Directory.Delete(unzipResult.UnzipDirectory, true);
            }
            catch
            {
            }
            return View();
        }
    }
}
