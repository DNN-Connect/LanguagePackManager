using System.Web.Mvc;
using Connect.LanguagePackManager.Core.Models.PackageLinks;
using Connect.LanguagePackManager.Core.Repositories;
using Connect.LanguagePackManager.Presentation.Common;

namespace Connect.LanguagePackManager.Presentation.Controllers
{
    public class LinksController : LanguagePackManagerMvcController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int linkId)
        {
            var link = PackageLinkRepository.Instance.GetPackageLink(ModuleContext.ModuleId, linkId);
            if (link == null) link = new Core.Models.PackageLinks.PackageLink();
            return View(link);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackageLink link)
        {

            //var link = PackageLinkRepository.Instance.GetPackageLink(ModuleContext.ModuleId, linkId);
            //if (link == null) link = new Core.Models.PackageLinks.PackageLink();
            //return View(link);
            return View("Index");
        }
    }
}
