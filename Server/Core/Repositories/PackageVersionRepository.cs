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
        public PackageVersion GetNextVersion(int packageId, string version)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteSingleOrDefault<PackageVersion>(System.Data.CommandType.Text,
                    "SELECT TOP 1 * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_PackageVersions WHERE PackageId=@0 AND Version>@1 ORDER BY Version ASC",
                    packageId, version);
            }

        }
    }
    public partial interface IPackageVersionRepository
    {
        PackageVersion GetNextVersion(int packageId, string version);
    }
}

