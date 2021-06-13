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
        public ActionResult Edit(int linkId, PackageLink link)
        {
            var existing = PackageLinkRepository.Instance.GetPackageLink(ModuleContext.ModuleId, linkId);
            if (existing == null)
            {
                existing = new PackageLink()
                {
                    ModuleId = ModuleContext.ModuleId
                };
            }
            existing.Name = link.Name.Trim();
            existing.OrgName = link.OrgName.Trim();
            existing.RepoName = link.RepoName.Trim();
            existing.AssetRegex = link.AssetRegex.Trim();
            if (linkId == -1)
            {
                PackageLinkRepository.Instance.AddPackageLink(existing.GetPackageLinkBase(), User.UserID);
            }
            else
            {
                PackageLinkRepository.Instance.UpdatePackageLink(existing.GetPackageLinkBase(), User.UserID);
            }

            return View("Index");
        }
    }
}
