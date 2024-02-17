using System;
using System.Data;
using System.Xml.Serialization;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.Texts
{

 [Serializable(), XmlRoot("Text")]
 public partial class Text
 {

  #region IPropertyAccess
  public override string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
  {
   switch (strPropertyName.ToLower()) {
    case "filepath": // NVarChar
     return PropertyAccess.FormatString(FilePath, strFormat);
    case "packageid": // Int
     return PackageId.ToString(strFormat, formatProvider);
    case "firstinversion": // VarChar
     return PropertyAccess.FormatString(FirstInVersion, strFormat);
    case "deprecatedinversion": // VarChar
     return PropertyAccess.FormatString(DeprecatedInVersion, strFormat);
    default:
       return base.GetProperty(strPropertyName, strFormat, formatProvider, accessingUser, accessLevel, ref propertyNotFound);
   }
  }
  #endregion

 }
}

