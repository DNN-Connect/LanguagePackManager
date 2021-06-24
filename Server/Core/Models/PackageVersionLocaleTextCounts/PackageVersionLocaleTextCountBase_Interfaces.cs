using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.PackageVersionLocaleTextCounts
{
    public partial class PackageVersionLocaleTextCountBase : IHydratable, IPropertyAccess
    {

        #region IHydratable

        public virtual void Fill(IDataReader dr)
        {
   PackageVersionId = Convert.ToInt32(Null.SetNull(dr["PackageVersionId"], PackageVersionId));
   LocaleId = Convert.ToInt32(Null.SetNull(dr["LocaleId"], LocaleId));
   NrTexts = Convert.ToInt32(Null.SetNull(dr["NrTexts"], NrTexts));
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
    case "packageversionid": // Int
     return PackageVersionId.ToString(strFormat, formatProvider);
    case "localeid": // Int
     return LocaleId.ToString(strFormat, formatProvider);
    case "nrtexts": // Int
     return NrTexts.ToString(strFormat, formatProvider);
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

