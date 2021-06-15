using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.Packages;

namespace Connect.LanguagePackManager.Core.Repositories
{
    public partial class PackageRepository : ServiceLocator<IPackageRepository, PackageRepository>, IPackageRepository
    {
        public Package FindPackage(int packageLinkId, string packageName)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteSingleOrDefault<Package>(System.Data.CommandType.Text,
                    "SELECT TOP 1 * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_Packages WHERE LinkId=@0 AND PackageName=@1",
                    packageLinkId, packageName);
            }
        }

        public Package FindPackage(string packageName, int moduleId)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteSingleOrDefault<Package>(System.Data.CommandType.Text,
                    "SELECT TOP 1 * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_Packages WHERE ModuleId=@0 AND PackageName=@1",
                    moduleId, packageName);
            }
        }
    }
    public partial interface IPackageRepository
    {
        Package FindPackage(int packageLinkId, string packageName);
        Package FindPackage(string packageName, int moduleId);
    }
}

