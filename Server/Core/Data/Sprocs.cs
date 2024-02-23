using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Connect.LanguagePackManager.Core.Models;
using Connect.LanguagePackManager.Core.Models.Translations;
using DotNetNuke.Data;
using DotNetNuke.Data.PetaPoco;

namespace Connect.LanguagePackManager.Core.Data
{
  public class Sprocs
  {
    // UPDATE dbo.Connect_LPM_PackageVersionLocaleTextCounts
    // SET NrTexts=-1, LastChange=y.LastModifiedOnDate
    // FROM dbo.Connect_LPM_PackageVersionLocaleTextCounts x
    // INNER JOIN
    // (SELECT
    //  pv.PackageVersionId,
    //  tr.Locale,
    //  MAX(tr.LastModifiedOnDate) LastModifiedOnDate
    // FROM dbo.Connect_LPM_Translations tr
    // INNER JOIN dbo.Connect_LPM_Texts t ON t.TextId=tr.TextId
    // INNER JOIN dbo.Connect_LPM_ResourceFiles rf on rf.ResourceFileId=t.ResourceFileId
    // INNER JOIN dbo.Connect_LPM_Packages p ON rf.PackageId=p.PackageId
    // INNER JOIN dbo.Connect_LPM_PackageVersions pv ON p.PackageId=pv.PackageId
    // GROUP BY tr.Locale, pv.PackageVersionId) y ON y.PackageVersionId=x.PackageVersionId AND y.Locale=x.LocaleId
    // WHERE y.LastModifiedOnDate > x.LastChange
    // ;  
    public static void DetectChangesPackageVersionLocaleTextCounts()
    {
      using (var context = DataContext.Instance())
      {
        context.Execute(System.Data.CommandType.StoredProcedure,
            "Connect_LPM_DetectChangesPackageVersionLocaleTextCounts"
            );
      }
    }

    // SELECT TOP 1
    //  t.LastModifiedOnDate
    // FROM dbo.vw_Connect_LPM_Translations t
    // WHERE t.PackageId=@PackageId
    // AND t.FirstInVersion <= @Version 
    // AND @Version < t.DeprecatedInVersion
    // AND (t.Locale=@LocaleMain OR t.Locale=@LocaleSpecific)
    // ORDER BY t.LastModifiedOnDate DESC;  
    public static DateTime GetLastEditTime(int packageId, string version, int localeMain, int localeSpecific)
    {
      using (var context = DataContext.Instance())
      {
        var res = context.ExecuteScalar<object>(System.Data.CommandType.StoredProcedure,
            "Connect_LPM_GetLastEditTime",
            packageId, version, localeMain, localeSpecific);
        if (res == null)
        {
          return DateTime.MinValue;
        }
        return (DateTime)res;
      }
    }

    // SELECT
    // *
    // FROM
    // (SELECT
    //  txt.FilePath,
    //  txt.TextKey,
    //  ISNULL(trs.TextValue, trg.TextValue) TextValue
    // FROM dbo.vw_Connect_LPM_Texts txt
    // LEFT JOIN dbo.Connect_LPM_Translations trg ON trg.TextId=txt.TextId AND trg.Locale=@LocaleGeneric
    // LEFT JOIN dbo.Connect_LPM_Translations trs ON trs.TextId=txt.TextId AND trs.Locale=@LocaleSpecific
    // WHERE txt.PackageId=@PackageId
    // AND txt.FirstInVersion <= @Version 
    // AND @Version < txt.DeprecatedInVersion) x
    // WHERE NOT x.TextValue IS NULL
    // ORDER BY FilePath, TextKey;  
    public static IEnumerable<PackTranslation> GetPack(int packageId, string version, int localeGeneric, int localeSpecific)
    {
      using (var context = DataContext.Instance())
      {
        return context.ExecuteQuery<PackTranslation>(System.Data.CommandType.StoredProcedure,
            "Connect_LPM_GetPack",
            packageId, version, localeGeneric, localeSpecific);
      }
    }

    // DECLARE @TempTable TABLE 
    // (
    //    PackageName nvarchar(128),
    //    Version varchar(15),
    //    Code varchar(10),
    //    TotalTexts int,
    //    NrTexts int,
    //    LastChange DATETIME
    // );
    // INSERT INTO @TempTable
    // SELECT
    //  p.PackageName,
    //  pv.Version,
    //  locs.Code,
    //  pv.NrTexts TotalTexts,
    //  x.NrTexts,
    //  x.LastChange
    // FROM dbo.Connect_LPM_PackageVersionLocaleTextCounts x
    // INNER JOIN dbo.Connect_LPM_PackageVersions pv ON pv.PackageVersionId=x.PackageVersionId
    // INNER JOIN dbo.Connect_LPM_Packages p ON p.PackageId=pv.PackageId
    // INNER JOIN dbo.Connect_LPM_PackageLinks pl ON pl.PackageLinkId=p.LinkId
    // INNER JOIN dbo.Modules m ON m.ModuleID=pl.ModuleId
    // INNER JOIN @Packages paramp ON paramp.PackageName=p.PackageName AND paramp.Version = pv.Version
    // INNER JOIN (SELECT l1.*
    // FROM dbo.Connect_LPM_Locales l1
    // INNER JOIN @Locales paraml ON paraml.LocaleCode=l1.Code OR paraml.GenericCode=l1.Code) locs ON locs.LocaleId=x.LocaleId
    // WHERE m.PortalID=@PortalId;
    // SELECT
    //  t.*
    // FROM @TempTable t
    // LEFT JOIN @TempTable t2 ON t2.PackageName=t.PackageName
    //   AND t2.Version=t.Version
    //   AND LEN(t2.Code)>LEN(t.Code)
    //   AND CHARINDEX(t.Code, t2.Code) = 1
    // WHERE t2.Code IS NULL;;  
    public static IEnumerable<GetStatDTO> GetStats(int PortalId, DataTable Packages, DataTable Locales)
    {
      var p1 = new SqlParameter()
      {
        ParameterName = "@Packages",
        TypeName = "dbo.Connect_LPM_GetStatsPackages",
        SqlDbType = SqlDbType.Structured,
        Value = Packages
      };
      var p2 = new SqlParameter()
      {
        ParameterName = "@Locales",
        TypeName = "dbo.Connect_LPM_GetStatsLocales",
        SqlDbType = SqlDbType.Structured,
        Value = Locales
      };

      using (var context = DataContext.Instance())
      {
        return context.ExecuteQuery<GetStatDTO>(CommandType.StoredProcedure,
            "Connect_LPM_GetStats",
            PortalId, p1, p2);
      }
    }

    // INSERT INTO dbo.Connect_LPM_PackageVersionLocaleTextCounts
    // ([PackageVersionId]
    // ,[LocaleId]
    // ,[NrTexts]
    // ,[LastChange])
    // SELECT
    //  pv.PackageVersionId,
    //  tr.Locale,
    //  -1,
    //  MAX(tr.LastModifiedOnDate) LastModifiedOnDate
    // FROM dbo.Connect_LPM_Translations tr
    // INNER JOIN dbo.Connect_LPM_Texts t ON t.TextId=tr.TextId
    // INNER JOIN dbo.Connect_LPM_ResourceFiles rf on rf.ResourceFileId=t.ResourceFileId
    // INNER JOIN dbo.Connect_LPM_Packages p ON rf.PackageId=p.PackageId
    // INNER JOIN dbo.Connect_LPM_PackageVersions pv ON p.PackageId=pv.PackageId
    // LEFT JOIN dbo.Connect_LPM_PackageVersionLocaleTextCounts x2 
    //   ON x2.PackageVersionId=pv.PackageVersionId
    //   AND x2.LocaleId=tr.Locale
    // GROUP BY tr.Locale, pv.PackageVersionId, x2.PackageVersionId
    // HAVING x2.PackageVersionId IS NULL;;  
    public static void InsertMissingPackageVersionLocaleTextCounts()
    {
      using (var context = DataContext.Instance())
      {
        context.Execute(System.Data.CommandType.StoredProcedure,
            "Connect_LPM_InsertMissingPackageVersionLocaleTextCounts"
            );
      }
    }

    // -- update the original nr of texts
    // UPDATE dbo.Connect_LPM_PackageVersions
    // SET NrTexts=x.NrTexts
    // FROM dbo.Connect_LPM_PackageVersions pv1
    // INNER JOIN
    // (SELECT
    // pv.PackageVersionId,
    // (SELECT COUNT(txt.TextId)
    // FROM dbo.vw_Connect_LPM_Texts txt
    // WHERE txt.PackageId=pv.PackageId
    // AND txt.FirstInVersion <= pv.Version
    // AND pv.Version < txt.DeprecatedInVersion) NrTexts
    // FROM dbo.vw_Connect_LPM_PackageVersions pv) x ON x.PackageVersionId=pv1.PackageVersionId
    // WHERE (pv1.PackageId=@PackageId OR @PackageId=-1);;  
    public static void RefreshNrOriginalTexts(int PackageId)
    {
      using (var context = DataContext.Instance())
      {
        context.Execute(System.Data.CommandType.StoredProcedure,
            "Connect_LPM_RefreshNrOriginalTexts",
            PackageId);
      }
    }

    // -- update the original nr of texts
    // EXEC dbo.Connect_LPM_RefreshNrOriginalTexts -1;
    // -- refresh translation counts
    // DELETE FROM dbo.Connect_LPM_PackageVersionLocaleTextCounts;
    // -- first get a list of available translations
    // EXEC dbo.Connect_LPM_InsertMissingPackageVersionLocaleTextCounts;
    // -- now get the nr texts
    // EXEC dbo.Connect_LPM_UpdatePackageVersionLocaleTextCounts;;  
    public static void RefreshNrTexts()
    {
      using (var context = DataContext.Instance())
      {
        context.Execute(System.Data.CommandType.StoredProcedure,
            "Connect_LPM_RefreshNrTexts"
            );
      }
    }

    // IF EXISTS (SELECT * FROM dbo.Connect_LPM_Translations WHERE TextId=@TextId AND Locale=@Locale)
    // BEGIN
    //  IF NOT EXISTS (SELECT * FROM dbo.Connect_LPM_Translations WHERE TextId=@TextId AND Locale=@Locale AND TextValue=@TextValue)
    //   UPDATE dbo.Connect_LPM_Translations
    //   SET TextValue=@TextValue, LastModifiedByUserID=@UserId, LastModifiedOnDate=GETDATE()
    //   WHERE TextId=@TextId AND Locale=@Locale
    // END
    // ELSE
    // BEGIN
    // INSERT INTO dbo.Connect_LPM_Translations
    //     ([TextId]
    //     ,[Locale]
    //     ,[TextValue]
    //     ,[CreatedByUserID]
    //     ,[CreatedOnDate]
    //     ,[LastModifiedByUserID]
    //     ,[LastModifiedOnDate])
    // VALUES
    //  (@TextId, @Locale, @TextValue, @UserId, GETDATE(), @UserId, GETDATE())
    // END;  
    public static void SetTranslation(int textId, int locale, string textValue, int userId)
    {
      using (var context = DataContext.Instance())
      {
        context.Execute(System.Data.CommandType.StoredProcedure,
            "Connect_LPM_SetTranslation",
            textId, locale, textValue, userId);
      }
    }

    // UPDATE dbo.Connect_LPM_PackageVersionLocaleTextCounts
    // SET NrTexts = (SELECT COUNT(txt.TextId)
    // FROM dbo.vw_Connect_LPM_Texts txt
    // LEFT JOIN dbo.Connect_LPM_Translations trg ON txt.TextId=trg.TextId AND trg.Locale=loc.GenericLocaleId
    // LEFT JOIN dbo.Connect_LPM_Translations trs ON txt.TextId=trs.TextId AND trs.Locale=loc.LocaleId
    // WHERE txt.PackageId=pv.PackageId
    // AND txt.FirstInVersion <= pv.Version
    // AND pv.Version < txt.DeprecatedInVersion
    // AND NOT ISNULL(trg.TextValue, trs.TextValue) IS NULL)
    // FROM dbo.Connect_LPM_PackageVersionLocaleTextCounts x
    // INNER JOIN dbo.Connect_LPM_PackageVersions pv ON pv.PackageVersionId=x.PackageVersionId
    // INNER JOIN dbo.Connect_LPM_Locales loc on loc.LocaleId=x.LocaleId
    // WHERE x.NrTexts=-1;;  
    public static void UpdatePackageVersionLocaleTextCounts()
    {
      using (var context = DataContext.Instance())
      {
        context.Execute(System.Data.CommandType.StoredProcedure,
            "Connect_LPM_UpdatePackageVersionLocaleTextCounts"
            );
      }
    }

  }
}
