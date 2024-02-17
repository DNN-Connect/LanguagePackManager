using System;
using System.Data;
using System.Xml.Serialization;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.Translations
{

 [Serializable(), XmlRoot("Translation")]
 public partial class Translation
 {

  #region IPropertyAccess
  public override string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
  {
   switch (strPropertyName.ToLower()) {
    case "packageid": // Int
     return PackageId.ToString(strFormat, formatProvider);
    case "filepath": // NVarChar
     return PropertyAccess.FormatString(FilePath, strFormat);
    case "textkey": // NVarChar
     return PropertyAccess.FormatString(TextKey, strFormat);
    case "firstinversion": // VarChar
     return PropertyAccess.FormatString(FirstInVersion, strFormat);
    case "deprecatedinversion": // VarChar
     return PropertyAccess.FormatString(DeprecatedInVersion, strFormat);
    case "createdbyuser": // NVarChar
     if (CreatedByUser == null)
     {
         return "";
     };
     return PropertyAccess.FormatString(CreatedByUser, strFormat);
    case "modifiedbyuser": // NVarChar
     if (ModifiedByUser == null)
     {
         return "";
     };
     return PropertyAccess.FormatString(ModifiedByUser, strFormat);
    default:
       return base.GetProperty(strPropertyName, strFormat, formatProvider, accessingUser, accessLevel, ref propertyNotFound);
   }
  }
  #endregion

 }
}

