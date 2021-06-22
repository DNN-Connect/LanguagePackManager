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

  #region IHydratable
  public override void Fill(IDataReader dr)
  {
   base.Fill(dr);
   PackageName = Convert.ToString(Null.SetNull(dr["PackageName"], PackageName));
   PackageType = Convert.ToString(Null.SetNull(dr["PackageType"], PackageType));
   Name = Convert.ToString(Null.SetNull(dr["Name"], Name));
   LastChecked = (DateTime)(Null.SetNull(dr["LastChecked"], LastChecked));
   ModuleId = Convert.ToInt32(Null.SetNull(dr["ModuleId"], ModuleId));
   PortalID = Convert.ToInt32(Null.SetNull(dr["PortalID"], PortalID));
  }
  #endregion

  #region IPropertyAccess
  public override string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
  {
   switch (strPropertyName.ToLower()) {
    case "packagename": // NVarChar
     return PropertyAccess.FormatString(PackageName, strFormat);
    case "packagetype": // NVarChar
     return PropertyAccess.FormatString(PackageType, strFormat);
    case "name": // NVarChar
     return PropertyAccess.FormatString(Name, strFormat);
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

