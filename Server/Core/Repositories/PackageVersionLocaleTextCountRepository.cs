using System.Collections.Generic;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.PackageVersionLocaleTextCounts;

namespace Connect.LanguagePackManager.Core.Repositories
{
    public partial class PackageVersionLocaleTextCountRepository : ServiceLocator<IPackageVersionLocaleTextCountRepository, PackageVersionLocaleTextCountRepository>, IPackageVersionLocaleTextCountRepository
    {
        public IEnumerable<PackageVersionLocaleTextCount> GetPackageVersionLocaleTextCounts(int localeId)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteQuery<PackageVersionLocaleTextCount>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}Connect_LPM_PackageVersionLocaleTextCounts WHERE LocaleId=@0",
                    localeId);
            }
        }
    }
    public partial interface IPackageVersionLocaleTextCountRepository
    {
        IEnumerable<PackageVersionLocaleTextCount> GetPackageVersionLocaleTextCounts(int localeId);
    }
}

