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
