using System;
using System.Data;
using System.Xml.Serialization;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Tokens;

namespace Connect.LanguagePackManager.Core.Models.PackageVersionLocaleTextCounts
{

 [Serializable(), XmlRoot("PackageVersionLocaleTextCount")]
 public partial class PackageVersionLocaleTextCount
 {

  #region IHydratable
  public override void Fill(IDataReader dr)
  {
   base.Fill(dr);
   OriginalNr = Convert.ToInt32(Null.SetNull(dr["OriginalNr"], OriginalNr));
   PackageId = Convert.ToInt32(Null.SetNull(dr["PackageId"], PackageId));
  }
  #endregion

  #region IPropertyAccess
  public override string GetProperty(string strPropertyName, string strFormat, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
  {
   switch (strPropertyName.ToLower()) {
    case "originalnr": // Int
     if (OriginalNr == null)
     {
         return "";
     };
     return ((int)OriginalNr).ToString(strFormat, formatProvider);
    case "packageid": // Int
     return PackageId.ToString(strFormat, formatProvider);
    default:
       return base.GetProperty(strPropertyName, strFormat, formatProvider, accessingUser, accessLevel, ref propertyNotFound);
   }
  }
  #endregion

 }
}

