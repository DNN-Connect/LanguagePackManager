using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.Packages;

namespace Connect.LanguagePackManager.Core.Repositories
{

	public partial class PackageRepository : ServiceLocator<IPackageRepository, PackageRepository>, IPackageRepository
 {
        protected override Func<IPackageRepository> GetFactory()
        {
            return () => new PackageRepository();
        }
        public IEnumerable<Package> GetPackages()
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Package>();
                return rep.Get();
            }
        }
        public IEnumerable<Package> GetPackagesByPackageLink(int linkId)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteQuery<Package>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_Packages WHERE LinkId=@0",
                    linkId);
            }
        }
        public Package GetPackage(int packageId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Package>();
                return rep.GetById(packageId);
            }
        }
        public PackageBase AddPackage(PackageBase package)
        {
            Requires.NotNull(package);
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageBase>();
                rep.Insert(package);
            }
            return package;
        }
        public void DeletePackage(PackageBase package)
        {
            Requires.NotNull(package);
            Requires.PropertyNotNegative(package, "PackageId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageBase>();
                rep.Delete(package);
            }
        }
        public void DeletePackage(int packageId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageBase>();
                rep.Delete("WHERE PackageId = @0", packageId);
            }
        }
        public void UpdatePackage(PackageBase package)
        {
            Requires.NotNull(package);
            Requires.PropertyNotNegative(package, "PackageId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageBase>();
                rep.Update(package);
            }
        } 
    }
    public partial interface IPackageRepository
    {
        IEnumerable<Package> GetPackages();
        IEnumerable<Package> GetPackagesByPackageLink(int linkId);
        Package GetPackage(int packageId);
        PackageBase AddPackage(PackageBase package);
        void DeletePackage(PackageBase package);
        void DeletePackage(int packageId);
        void UpdatePackage(PackageBase package);
    }
}

