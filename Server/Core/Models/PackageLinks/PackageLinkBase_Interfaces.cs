using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.PackageLinks
{
    public partial class PackageLinkBase : IPropertyAccess
    {
        #region IPropertyAccess
        public virtual string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
        {
            switch (strPropertyName.ToLower())
            {
    case "packagelinkid": // Int
     return PackageLinkId.ToString(strFormat, formatProvider);
    case "moduleid": // Int
     return ModuleId.ToString(strFormat, formatProvider);
    case "name": // NVarChar
     return PropertyAccess.FormatString(Name, strFormat);
    case "orgname": // NVarChar
     return PropertyAccess.FormatString(OrgName, strFormat);
    case "reponame": // NVarChar
     return PropertyAccess.FormatString(RepoName, strFormat);
    case "assetregex": // NVarChar
     return PropertyAccess.FormatString(AssetRegex, strFormat);
    case "lastchecked": // DateTime
     if (LastChecked == null)
     {
         return "";
     };
     return ((DateTime)LastChecked).ToString(strFormat, formatProvider);
    case "lastdownloadedversion": // VarChar
     if (LastDownloadedVersion == null)
     {
         return "";
     };
     return PropertyAccess.FormatString(LastDownloadedVersion, strFormat);
    case "isresourcesrepo": // Bit
     return IsResourcesRepo.ToString();
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

