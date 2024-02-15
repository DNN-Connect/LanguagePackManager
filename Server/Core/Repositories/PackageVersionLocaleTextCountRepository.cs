using System.Collections.Generic;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.PackageVersionLocaleTextCounts;
using System.Linq;

namespace Connect.LanguagePackManager.Core.Repositories
{
  public partial class PackageVersionLocaleTextCountRepository : ServiceLocator<IPackageVersionLocaleTextCountRepository, PackageVersionLocaleTextCountRepository>, IPackageVersionLocaleTextCountRepository
  {
    public IEnumerable<PackageVersionLocaleTextCount> GetPackageVersionLocaleTextCounts(int localeId)
    {
      using (var context = DataContext.Instance())
      {
        return context.ExecuteQuery<PackageVersionLocaleTextCount>(System.Data.CommandType.Text,
            "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_PackageVersionLocaleTextCounts WHERE LocaleId=@0",
            localeId);
      }
    }

    private class GetAvailableComponentsReturn
    {
      public int PackageId { get; set; }
    }
    public IEnumerable<int> GetAvailableComponents(int localeId)
    {
      using (var context = DataContext.Instance())
      {
        var sql = @"SELECT DISTINCT
 pv.PackageId
FROM
(SELECT DISTINCT 
 y.PackageVersionId
FROM
(SELECT
 x.PackageVersionId
FROM {databaseOwner}{objectQualifier}Connect_LPM_PackageVersionLocaleTextCounts x
WHERE x.LocaleId=@0 AND x.NrTexts > 0
UNION
SELECT
 x.PackageVersionId
FROM {databaseOwner}{objectQualifier}Connect_LPM_PackageVersionLocaleTextCounts x
INNER JOIN {databaseOwner}{objectQualifier}Connect_LPM_Locales lprec on lprec.GenericLocaleId=x.LocaleId
WHERE lprec.LocaleId=@0 AND x.NrTexts > 0) y) z
INNER JOIN {databaseOwner}{objectQualifier}Connect_LPM_PackageVersions pv ON pv.PackageVersionId=z.PackageVersionId";

        return context.ExecuteQuery<GetAvailableComponentsReturn>(System.Data.CommandType.Text,
            sql,
            localeId)
            .Select(r => r.PackageId);
      }
    }
  }
  public partial interface IPackageVersionLocaleTextCountRepository
  {
    IEnumerable<PackageVersionLocaleTextCount> GetPackageVersionLocaleTextCounts(int localeId);
    IEnumerable<int> GetAvailableComponents(int localeId);
  }
}

