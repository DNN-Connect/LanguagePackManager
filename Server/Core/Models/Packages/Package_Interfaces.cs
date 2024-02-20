using System;
using System.Data;
using System.Xml.Serialization;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.Packages
{

 [Serializable(), XmlRoot("Package")]
 public partial class Package
 {

  #region IPropertyAccess
  public override string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
  {
   switch (strPropertyName.ToLower()) {
    case "name": // NVarChar
     return PropertyAccess.FormatString(Name, strFormat);
    case "isresourcesrepo": // Bit
     return IsResourcesRepo.ToString();
    case "lastchecked": // DateTime
     if (LastChecked == null)
     {
         return "";
     };
     return ((DateTime)LastChecked).ToString(strFormat, formatProvider);
    case "moduleid": // Int
     return ModuleId.ToString(strFormat, formatProvider);
    case "portalid": // Int
     if (PortalID == null)
     {
         return "";
     };
     return ((int)PortalID).ToString(strFormat, formatProvider);
    default:
       return base.GetProperty(strPropertyName, strFormat, formatProvider, accessingUser, accessLevel, ref propertyNotFound);
   }
  }
  #endregion

 }
}

