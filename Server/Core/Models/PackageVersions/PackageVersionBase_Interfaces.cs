using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.PackageVersions
{
    public partial class PackageVersionBase : IHydratable, IPropertyAccess
    {

        #region IHydratable

        public virtual void Fill(IDataReader dr)
        {
   PackageVersionId = Convert.ToInt32(Null.SetNull(dr["PackageVersionId"], PackageVersionId));
   PackageId = Convert.ToInt32(Null.SetNull(dr["PackageId"], PackageId));
   ContainedInPackageVersionId = Convert.ToInt32(Null.SetNull(dr["ContainedInPackageVersionId"], ContainedInPackageVersionId));
   Version = Convert.ToString(Null.SetNull(dr["Version"], Version));
   ReleaseDate = (DateTime)(Null.SetNull(dr["ReleaseDate"], ReleaseDate));
   Downloaded = (DateTime)(Null.SetNull(dr["Downloaded"], Downloaded));
   NrTexts = Convert.ToInt32(Null.SetNull(dr["NrTexts"], NrTexts));
        }

        [IgnoreColumn()]
        public int KeyID
        {
            get { return PackageVersionId; }
            set { PackageVersionId = value; }
        }
        #endregion

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

