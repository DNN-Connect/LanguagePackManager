using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.Packages
{
    public partial class PackageBase : IHydratable, IPropertyAccess
    {

        #region IHydratable

        public virtual void Fill(IDataReader dr)
        {
   PackageId = Convert.ToInt32(Null.SetNull(dr["PackageId"], PackageId));
   LinkId = Convert.ToInt32(Null.SetNull(dr["LinkId"], LinkId));
   PackageName = Convert.ToString(Null.SetNull(dr["PackageName"], PackageName));
   FriendlyName = Convert.ToString(Null.SetNull(dr["FriendlyName"], FriendlyName));
   PackageType = Convert.ToString(Null.SetNull(dr["PackageType"], PackageType));
   InstallPath = Convert.ToString(Null.SetNull(dr["InstallPath"], InstallPath));
   LastVersion = Convert.ToString(Null.SetNull(dr["LastVersion"], LastVersion));
        }

        [IgnoreColumn()]
        public int KeyID
        {
            get { return PackageId; }
            set { PackageId = value; }
        }
        #endregion

        #region IPropertyAccess
        public virtual string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
        {
            switch (strPropertyName.ToLower())
            {
    case "packageid": // Int
     return PackageId.ToString(strFormat, formatProvider);
    case "linkid": // Int
     return LinkId.ToString(strFormat, formatProvider);
    case "packagename": // NVarChar
     return PropertyAccess.FormatString(PackageName, strFormat);
    case "friendlyname": // NVarChar
     return PropertyAccess.FormatString(FriendlyName, strFormat);
    case "packagetype": // NVarChar
     return PropertyAccess.FormatString(PackageType, strFormat);
    case "installpath": // NVarChar
     if (InstallPath == null)
     {
         return "";
     };
     return PropertyAccess.FormatString(InstallPath, strFormat);
    case "lastversion": // VarChar
     if (LastVersion == null)
     {
         return "";
     };
     return PropertyAccess.FormatString(LastVersion, strFormat);
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

