using System;
using System.Data;
using System.Xml.Serialization;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.PackageVersions
{

 [Serializable(), XmlRoot("PackageVersion")]
 public partial class PackageVersion
 {

  #region IPropertyAccess
  public override string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
  {
   switch (strPropertyName.ToLower()) {
    case "packagename": // NVarChar
     return PropertyAccess.FormatString(PackageName, strFormat);
    case "friendlyname": // NVarChar
     return PropertyAccess.FormatString(FriendlyName, strFormat);
    case "packagetype": // NVarChar
     return PropertyAccess.FormatString(PackageType, strFormat);
    case "packagelinkname": // NVarChar
     return PropertyAccess.FormatString(PackageLinkName, strFormat);
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

