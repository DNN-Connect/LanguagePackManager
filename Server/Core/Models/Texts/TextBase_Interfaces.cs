using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.Texts
{
    public partial class TextBase : IPropertyAccess
    {
        #region IPropertyAccess
        public virtual string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
        {
            switch (strPropertyName.ToLower())
            {
    case "textid": // Int
     return TextId.ToString(strFormat, formatProvider);
    case "packageversionid": // Int
     return PackageVersionId.ToString(strFormat, formatProvider);
    case "resourcefileid": // Int
     return ResourceFileId.ToString(strFormat, formatProvider);
    case "textkey": // NVarChar
     return PropertyAccess.FormatString(TextKey, strFormat);
    case "originalvalue": // NVarCharMax
     if (OriginalValue == null)
     {
         return "";
     };
     return PropertyAccess.FormatString(OriginalValue, strFormat);
    case "deprecatedinversionid": // Int
     if (DeprecatedInVersionId == null)
     {
         return "";
     };
     return ((int)DeprecatedInVersionId).ToString(strFormat, formatProvider);
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

