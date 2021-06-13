using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.PackageLinks;

namespace Connect.LanguagePackManager.Core.Repositories
{

	public partial class PackageLinkRepository : ServiceLocator<IPackageLinkRepository, PackageLinkRepository>, IPackageLinkRepository
 {
        protected override Func<IPackageLinkRepository> GetFactory()
        {
            return () => new PackageLinkRepository();
        }
        public IEnumerable<PackageLink> GetPackageLinks(int moduleId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageLink>();
                return rep.Get(moduleId);
            }
        }
        public PackageLink GetPackageLink(int moduleId, int packageLinkId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageLink>();
                return rep.GetById(packageLinkId, moduleId);
            }
        }
        public PackageLinkBase AddPackageLink(PackageLinkBase packageLink, int userId)
        {
            Requires.NotNull(packageLink);
            Requires.PropertyNotNegative(packageLink, "ModuleId");
            packageLink.CreatedByUserID = userId;
            packageLink.CreatedOnDate = DateTime.Now;
            packageLink.LastModifiedByUserID = userId;
            packageLink.LastModifiedOnDate = DateTime.Now;
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageLinkBase>();
                rep.Insert(packageLink);
            }
            return packageLink;
        }
        public void DeletePackageLink(PackageLinkBase packageLink)
        {
            Requires.NotNull(packageLink);
            Requires.PropertyNotNegative(packageLink, "PackageLinkId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageLinkBase>();
                rep.Delete(packageLink);
            }
        }
        public void DeletePackageLink(int moduleId, int packageLinkId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageLinkBase>();
                rep.Delete("WHERE ModuleId = @0 AND PackageLinkId = @1", moduleId, packageLinkId);
            }
        }
        public void UpdatePackageLink(PackageLinkBase packageLink, int userId)
        {
            Requires.NotNull(packageLink);
            Requires.PropertyNotNegative(packageLink, "PackageLinkId");
            packageLink.LastModifiedByUserID = userId;
            packageLink.LastModifiedOnDate = DateTime.Now;
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageLinkBase>();
                rep.Update(packageLink);
            }
        } 
    }
    public partial interface IPackageLinkRepository
    {
        IEnumerable<PackageLink> GetPackageLinks(int moduleId);
        PackageLink GetPackageLink(int moduleId, int packageLinkId);
        PackageLinkBase AddPackageLink(PackageLinkBase packageLink, int userId);
        void DeletePackageLink(PackageLinkBase packageLink);
        void DeletePackageLink(int moduleId, int packageLinkId);
        void UpdatePackageLink(PackageLinkBase packageLink, int userId);
    }
}

