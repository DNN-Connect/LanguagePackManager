using System;
using System.Collections.Generic;
using Connect.LanguagePackManager.Core.Models.Translations;
using DotNetNuke.Data;

namespace Connect.LanguagePackManager.Core.Data
{
    public class Sprocs
    {
        // SELECT TOP 1
        //  t.LastModifiedOnDate
        // FROM dbo.vw_Connect_LPM_Translations t
        // WHERE t.PackageId=@PackageId
        // AND t.FirstInVersion <= @Version 
        // AND @Version < t.DeprecatedInVersion
        // AND (t.Locale=@LocaleMain OR t.Locale=@LocaleSpecific)
        // ORDER BY t.LastModifiedOnDate DESC
        // 
        // ;  
        public static DateTime GetLastEditTime(int packageId, string version, int localeMain, int localeSpecific)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteScalar<DateTime>(System.Data.CommandType.StoredProcedure,
                    "Connect_LPM_GetLastEditTime",
                    packageId, version, localeMain, localeSpecific);
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
        // ORDER BY FilePath, TextKey
        // ;  
        public static IEnumerable<PackTranslation> GetPack(int packageId, string version, int localeGeneric, int localeSpecific)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteQuery<PackTranslation>(System.Data.CommandType.StoredProcedure,
                    "Connect_LPM_GetPack",
                    packageId, version, localeGeneric, localeSpecific);
            }
        }

        // DELETE FROM dbo.Connect_LPM_PackageVersionLocaleTextCounts
        // WHERE LocaleId=@LocaleId;
        // 
        // INSERT INTO dbo.Connect_LPM_PackageVersionLocaleTextCounts
        //     ([PackageVersionId]
        //     ,[LocaleId]
        //     ,[NrTexts])
        // SELECT
        // pv.PackageVersionId,
        // loc.LocaleId,
        // (SELECT COUNT(txt.TextId)
        // FROM dbo.vw_Connect_LPM_Texts txt
        // LEFT JOIN dbo.Connect_LPM_Translations trg ON txt.TextId=trg.TextId AND trg.Locale=loc.GenericLocaleId
        // LEFT JOIN dbo.Connect_LPM_Translations trs ON txt.TextId=trs.TextId AND trs.Locale=loc.LocaleId
        // WHERE txt.PackageId=pv.PackageId
        // AND txt.FirstInVersion <= pv.Version
        // AND pv.Version < txt.DeprecatedInVersion
        // AND NOT ISNULL(trg.TextValue, trs.TextValue) IS NULL) NrTexts
        // FROM dbo.vw_Connect_LPM_PackageVersions pv
        // INNER JOIN dbo.Connect_LPM_Locales loc ON loc.LocaleId=@LocaleId;
        // 
        // ;  
        public static void RefreshLocaleTextCount(int localeId)
        {
            using (var context = DataContext.Instance())
            {
                context.Execute(System.Data.CommandType.StoredProcedure,
                    "Connect_LPM_RefreshLocaleTextCount",
                    localeId);
            }
        }

        // UPDATE dbo.Connect_LPM_PackageVersions
        // SET NrTexts=x.NrTexts
        // FROM dbo.Connect_LPM_PackageVersions pv1
        // INNER JOIN
        // (SELECT
        // pv.PackageId,
        // (SELECT COUNT(txt.TextId)
        // FROM dbo.vw_Connect_LPM_Texts txt
        // WHERE txt.PackageId=pv.PackageId
        // AND txt.FirstInVersion <= pv.Version
        // AND pv.Version < txt.DeprecatedInVersion) NrTexts
        // FROM dbo.vw_Connect_LPM_PackageVersions pv) x ON x.PackageId=pv1.PackageId;
        // 
        // DELETE FROM dbo.Connect_LPM_PackageVersionLocaleTextCounts;
        // 
        // INSERT INTO dbo.Connect_LPM_PackageVersionLocaleTextCounts
        // ([PackageVersionId],[LocaleId],[NrTexts])
        // SELECT
        // pv.PackageVersionId,
        // loc.LocaleId,
        // (SELECT COUNT(txt.TextId)
        // FROM dbo.vw_Connect_LPM_Texts txt
        // LEFT JOIN dbo.Connect_LPM_Translations trg ON txt.TextId=trg.TextId AND trg.Locale=loc.GenericLocaleId
        // LEFT JOIN dbo.Connect_LPM_Translations trs ON txt.TextId=trs.TextId AND trs.Locale=loc.LocaleId
        // WHERE txt.PackageId=pv.PackageId
        // AND txt.FirstInVersion <= pv.Version
        // AND pv.Version < txt.DeprecatedInVersion
        // AND NOT ISNULL(trg.TextValue, trs.TextValue) IS NULL) NrTexts
        // FROM dbo.vw_Connect_LPM_PackageVersions pv
        // INNER JOIN dbo.Connect_LPM_Locales loc ON 1=1
        // WHERE NOT loc.GenericLocaleId IS NULL;
        // 
        // ;  
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

    }
}
