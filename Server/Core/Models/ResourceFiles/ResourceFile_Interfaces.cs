using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.ResourceFiles
{
    public partial class ResourceFile : IHydratable, IPropertyAccess
    {

        #region IHydratable

        public virtual void Fill(IDataReader dr)
        {
   ResourceFileId = Convert.ToInt32(Null.SetNull(dr["ResourceFileId"], ResourceFileId));
   PackageId = Convert.ToInt32(Null.SetNull(dr["PackageId"], PackageId));
   FilePath = Convert.ToString(Null.SetNull(dr["FilePath"], FilePath));
        }

        [IgnoreColumn()]
        public int KeyID
        {
            get { return ResourceFileId; }
            set { ResourceFileId = value; }
        }
        #endregion

        #region IPropertyAccess
        public virtual string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
        {
            switch (strPropertyName.ToLower())
            {
    case "resourcefileid": // Int
     return ResourceFileId.ToString(strFormat, formatProvider);
    case "packageid": // Int
     return PackageId.ToString(strFormat, formatProvider);
    case "filepath": // NVarChar
     return PropertyAccess.FormatString(FilePath, strFormat);
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

