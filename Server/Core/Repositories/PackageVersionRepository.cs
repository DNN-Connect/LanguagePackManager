using System.Collections.Generic;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.PackageVersions;

namespace Connect.LanguagePackManager.Core.Repositories
{
    public partial class PackageVersionRepository : ServiceLocator<IPackageVersionRepository, PackageVersionRepository>, IPackageVersionRepository
    {
        public PackageVersion GetNextVersion(int packageId, string version)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteSingleOrDefault<PackageVersion>(System.Data.CommandType.Text,
                    "SELECT TOP 1 * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_PackageVersions WHERE PackageId=@0 AND Version>@1 ORDER BY Version ASC",
                    packageId, version);
            }
        }
        public PackageVersion GetPackageVersion(int portalId, string packageName, string version)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteSingleOrDefault<PackageVersion>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_PackageVersions WHERE PortalID=@0 AND PackageName=@1 AND Version=@2",
                    portalId, packageName, version);
            }
        }
        public IEnumerable<PackageVersion> GetChildPackageVersions(int packageVersionId)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteQuery<PackageVersion>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_PackageVersions WHERE ContainedInPackageVersionId=@0",
                    packageVersionId);
            }
        }
        public IEnumerable<PackageVersion> GetPackageVersions(int moduleId)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteQuery<PackageVersion>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_PackageVersions WHERE ModuleId=@0",
                    moduleId);
            }
        }
    }
    public partial interface IPackageVersionRepository
    {
        PackageVersion GetNextVersion(int packageId, string version);
        PackageVersion GetPackageVersion(int portalId, string packageName, string version);
        IEnumerable<PackageVersion> GetChildPackageVersions(int packageVersionId);
        IEnumerable<PackageVersion> GetPackageVersions(int moduleId);
    }
}

