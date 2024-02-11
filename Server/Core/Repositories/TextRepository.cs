using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.Texts;
using System.Data.SqlClient;
using System;
using DotNetNuke.Data;
using DotNetNuke.Data.PetaPoco;

namespace Connect.LanguagePackManager.Core.Repositories
{
  public partial class TextRepository : ServiceLocator<ITextRepository, TextRepository>, ITextRepository
  {
    public TextBase AddText(int packageVersionId, int resourceFileId, string key, string value)
    {
      var text = new TextBase()
      {
        PackageVersionId = packageVersionId,
        ResourceFileId = resourceFileId,
        TextKey = key,
        OriginalValue = value
      };
      return AddText(text);
    }

    public void RefreshNrTexts()
    {
      // We have to bypass the regular DB Context as we need to be able to set the timeout. This SPROC can take minutes to complete.
      var dbInst = DataProvider.Instance();
      PetaPocoHelper.ExecuteNonQuery(
          dbInst.ConnectionString,
          System.Data.CommandType.StoredProcedure,
          (int)TimeSpan.FromMinutes(30).TotalSeconds,
          dbInst.DatabaseOwner + dbInst.ObjectQualifier + "Connect_LPM_RefreshNrTexts");
    }
  }
  public partial interface ITextRepository
  {
    TextBase AddText(int packageVersionId, int resourceFileId, string key, string value);
    void RefreshNrTexts();
  }
}

