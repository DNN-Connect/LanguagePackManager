using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.PackageLinks
{
    public partial class PackageLinkBase : IHydratable, IPropertyAccess
    {

        #region IHydratable

        public virtual void Fill(IDataReader dr)
        {
            FillAuditFields(dr);
   PackageLinkId = Convert.ToInt32(Null.SetNull(dr["PackageLinkId"], PackageLinkId));
   ModuleId = Convert.ToInt32(Null.SetNull(dr["ModuleId"], ModuleId));
   Name = Convert.ToString(Null.SetNull(dr["Name"], Name));
   OrgName = Convert.ToString(Null.SetNull(dr["OrgName"], OrgName));
   RepoName = Convert.ToString(Null.SetNull(dr["RepoName"], RepoName));
   AssetRegex = Convert.ToString(Null.SetNull(dr["AssetRegex"], AssetRegex));
   LastChecked = (DateTime)(Null.SetNull(dr["LastChecked"], LastChecked));
   LastDownloadedVersion = Convert.ToString(Null.SetNull(dr["LastDownloadedVersion"], LastDownloadedVersion));
        }

        [IgnoreColumn()]
        public int KeyID
        {
            get { return PackageLinkId; }
            set { PackageLinkId = value; }
        }
        #endregion

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

