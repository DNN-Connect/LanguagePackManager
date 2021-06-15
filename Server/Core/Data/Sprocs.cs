using System;
using System.Collections.Generic;
using DotNetNuke.Data;

namespace Connect.LanguagePackManager.Core.Data
{
    public class Sprocs
    {
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
        public static void SetTranslation(int textId, string locale, string textValue, int userId)
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
