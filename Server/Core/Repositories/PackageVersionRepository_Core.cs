using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.PackageVersions;

namespace Connect.LanguagePackManager.Core.Repositories
{

	public partial class PackageVersionRepository : ServiceLocator<IPackageVersionRepository, PackageVersionRepository>, IPackageVersionRepository
 {
        protected override Func<IPackageVersionRepository> GetFactory()
        {
            return () => new PackageVersionRepository();
        }
        public IEnumerable<PackageVersion> GetPackageVersions()
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageVersion>();
                return rep.Get();
            }
        }
        public IEnumerable<PackageVersion> GetPackageVersionsByPackage(int packageId)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteQuery<PackageVersion>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_PackageVersions WHERE PackageId=@0",
                    packageId);
            }
        }
        public PackageVersion GetPackageVersion(int packageVersionId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageVersion>();
                return rep.GetById(packageVersionId);
            }
        }
        public PackageVersionBase AddPackageVersion(PackageVersionBase packageVersion)
        {
            Requires.NotNull(packageVersion);
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageVersionBase>();
                rep.Insert(packageVersion);
            }
            return packageVersion;
        }
        public void DeletePackageVersion(PackageVersionBase packageVersion)
        {
            Requires.NotNull(packageVersion);
            Requires.PropertyNotNegative(packageVersion, "PackageVersionId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageVersionBase>();
                rep.Delete(packageVersion);
            }
        }
        public void DeletePackageVersion(int packageVersionId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageVersionBase>();
                rep.Delete("WHERE PackageVersionId = @0", packageVersionId);
            }
        }
        public void UpdatePackageVersion(PackageVersionBase packageVersion)
        {
            Requires.NotNull(packageVersion);
            Requires.PropertyNotNegative(packageVersion, "PackageVersionId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageVersionBase>();
                rep.Update(packageVersion);
            }
        } 
    }
    public partial interface IPackageVersionRepository
    {
        IEnumerable<PackageVersion> GetPackageVersions();
        IEnumerable<PackageVersion> GetPackageVersionsByPackage(int packageId);
        PackageVersion GetPackageVersion(int packageVersionId);
        PackageVersionBase AddPackageVersion(PackageVersionBase packageVersion);
        void DeletePackageVersion(PackageVersionBase packageVersion);
        void DeletePackageVersion(int packageVersionId);
        void UpdatePackageVersion(PackageVersionBase packageVersion);
    }
}

