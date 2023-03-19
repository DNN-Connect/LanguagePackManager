using Connect.LanguagePackManager.Core.Common;
using Connect.LanguagePackManager.Core.Helpers;
using Connect.LanguagePackManager.Core.Services.Packages;
using Connect.LanguagePackManager.Presentation.Common;
using System.IO;
using System.Web.Mvc;

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
      manifest.Process(generic, User.UserID);

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
