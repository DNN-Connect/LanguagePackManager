using System.Linq;
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
            link.Name = link.Name.Trim();
            link.OrgName = link.OrgName.Trim().ToLowerInvariant();
            link.RepoName = link.RepoName.Trim().ToLowerInvariant();
            link.AssetRegex = link.AssetRegex.Trim();

            var existing = PackageLinkRepository.Instance.GetPackageLink(ModuleContext.ModuleId, linkId);
            if (existing == null)
            {
                var clash = PackageLinkRepository.Instance.GetPackageLinks(ModuleContext.ModuleId).Where(p => p.OrgName == link.OrgName && p.RepoName == link.RepoName);
                if (clash.Count() > 0)
                {
                    throw new System.Exception("This link already exists");
                }
                existing = new PackageLink()
                {
                    ModuleId = ModuleContext.ModuleId
                };
            }
            existing.Name = link.Name;
            existing.OrgName = link.OrgName;
            existing.RepoName = link.RepoName;
            existing.AssetRegex = link.AssetRegex;
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
