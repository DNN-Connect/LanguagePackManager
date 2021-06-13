using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.Translations
{
    public partial class TranslationBase : IHydratable, IPropertyAccess
    {

        #region IHydratable

        public virtual void Fill(IDataReader dr)
        {
            FillAuditFields(dr);
   TextId = Convert.ToInt32(Null.SetNull(dr["TextId"], TextId));
   Locale = Convert.ToString(Null.SetNull(dr["Locale"], Locale));
   TextValue = Convert.ToString(Null.SetNull(dr["TextValue"], TextValue));
        }

        [IgnoreColumn()]
        public int KeyID
        {
            get { return Null.NullInteger; }
            set { }
        }
        #endregion

        #region IPropertyAccess
        public virtual string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
        {
            switch (strPropertyName.ToLower())
            {
    case "textid": // Int
     return TextId.ToString(strFormat, formatProvider);
    case "locale": // NVarChar
     return PropertyAccess.FormatString(Locale, strFormat);
    case "textvalue": // NVarCharMax
     if (TextValue == null)
     {
         return "";
     };
     return PropertyAccess.FormatString(TextValue, strFormat);
                default:
                    propertyNotFound = true;
                    break;
            }

            return Null.NullString;
        }

        [IgnoreColumn()]
        public CacheLevel Cacheability
        {
            get { return CacheLevel.fullyCacheable; }
        }
        #endregion

    }
}

