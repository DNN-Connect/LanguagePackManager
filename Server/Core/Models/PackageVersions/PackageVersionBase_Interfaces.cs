using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.PackageVersions
{
    public partial class PackageVersionBase : IPropertyAccess
    {
        #region IPropertyAccess
        public virtual string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
        {
            switch (strPropertyName.ToLower())
            {
    case "packageversionid": // Int
     return PackageVersionId.ToString(strFormat, formatProvider);
    case "packageid": // Int
     return PackageId.ToString(strFormat, formatProvider);
    case "containedinpackageversionid": // Int
     if (ContainedInPackageVersionId == null)
     {
         return "";
     };
     return ((int)ContainedInPackageVersionId).ToString(strFormat, formatProvider);
    case "version": // VarChar
     return PropertyAccess.FormatString(Version, strFormat);
    case "releasedate": // DateTime
     return ReleaseDate.ToString(strFormat, formatProvider);
    case "downloaded": // DateTime
     return Downloaded.ToString(strFormat, formatProvider);
    case "nrtexts": // Int
     if (NrTexts == null)
     {
         return "";
     };
     return ((int)NrTexts).ToString(strFormat, formatProvider);
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

