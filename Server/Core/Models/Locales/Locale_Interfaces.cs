using System;
using System.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.Locales
{
    public partial class Locale : IHydratable, IPropertyAccess
    {

        #region IHydratable

        public virtual void Fill(IDataReader dr)
        {
   LocaleId = Convert.ToInt32(Null.SetNull(dr["LocaleId"], LocaleId));
   Code = Convert.ToString(Null.SetNull(dr["Code"], Code));
        }

        [IgnoreColumn()]
        public int KeyID
        {
            get { return LocaleId; }
            set { LocaleId = value; }
        }
        #endregion

        #region IPropertyAccess
        public virtual string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
        {
            switch (strPropertyName.ToLower())
            {
    case "localeid": // Int
     return LocaleId.ToString(strFormat, formatProvider);
    case "code": // VarChar
     return PropertyAccess.FormatString(Code, strFormat);
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

