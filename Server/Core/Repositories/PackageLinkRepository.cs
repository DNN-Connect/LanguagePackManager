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
        public IEnumerable<PackageLink> GetPackageLinks()
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageLink>();
                return rep.Get();
            }
        }
    }
    public partial interface IPackageLinkRepository
    {
        IEnumerable<PackageLink> GetPackageLinks();
    }
}

