using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.Texts
{
    public partial class TextBase : IHydratable, IPropertyAccess
    {

        #region IHydratable

        public virtual void Fill(IDataReader dr)
        {
   TextId = Convert.ToInt32(Null.SetNull(dr["TextId"], TextId));
   PackageVersionId = Convert.ToInt32(Null.SetNull(dr["PackageVersionId"], PackageVersionId));
   ResourceFileId = Convert.ToInt32(Null.SetNull(dr["ResourceFileId"], ResourceFileId));
   TextKey = Convert.ToString(Null.SetNull(dr["TextKey"], TextKey));
   OriginalValue = Convert.ToString(Null.SetNull(dr["OriginalValue"], OriginalValue));
   DeprecatedInVersionId = Convert.ToInt32(Null.SetNull(dr["DeprecatedInVersionId"], DeprecatedInVersionId));
        }

        [IgnoreColumn()]
        public int KeyID
        {
            get { return TextId; }
            set { TextId = value; }
        }
        #endregion

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

