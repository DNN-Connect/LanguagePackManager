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

  #region IHydratable
  public override void Fill(IDataReader dr)
  {
   base.Fill(dr);
   PackageId = Convert.ToInt32(Null.SetNull(dr["PackageId"], PackageId));
   FilePath = Convert.ToString(Null.SetNull(dr["FilePath"], FilePath));
   TextKey = Convert.ToString(Null.SetNull(dr["TextKey"], TextKey));
   FirstInVersion = Convert.ToString(Null.SetNull(dr["FirstInVersion"], FirstInVersion));
   DeprecatedInVersion = Convert.ToString(Null.SetNull(dr["DeprecatedInVersion"], DeprecatedInVersion));
   CreatedByUser = Convert.ToString(Null.SetNull(dr["CreatedByUser"], CreatedByUser));
   ModifiedByUser = Convert.ToString(Null.SetNull(dr["ModifiedByUser"], ModifiedByUser));
  }
  #endregion

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

