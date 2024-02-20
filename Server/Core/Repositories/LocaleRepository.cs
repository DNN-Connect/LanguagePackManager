using Connect.LanguagePackManager.Core.Models.Locales;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using System.Collections.Generic;

namespace Connect.LanguagePackManager.Core.Repositories
{
  public partial class LocaleRepository : ServiceLocator<ILocaleRepository, LocaleRepository>, ILocaleRepository
  {
    public Locale GetOrCreateLocale(string code)
    {
      using (var context = DataContext.Instance())
      {
        var res = context.ExecuteSingleOrDefault<Locale>(System.Data.CommandType.Text, @"SELECT * FROM {databaseOwner}{objectQualifier}Connect_LPM_Locales WHERE Code=@0;", code);
        if (res == null)
        {
          if (code.IndexOf("-") > 0)
          {
            var genericCode = code.Substring(0, code.IndexOf("-"));
            var genericLocale = this.GetOrCreateLocale(genericCode);
            context.Execute(System.Data.CommandType.Text, @"IF NOT EXISTS (SELECT * FROM {databaseOwner}{objectQualifier}Connect_LPM_Locales WHERE Code=@0)
 INSERT INTO {databaseOwner}{objectQualifier}Connect_LPM_Locales (Code, GenericLocaleId) VALUES (@0, @1);", code, genericLocale.LocaleId);
          }
          else
          {
            context.Execute(System.Data.CommandType.Text, @"IF NOT EXISTS (SELECT * FROM {databaseOwner}{objectQualifier}Connect_LPM_Locales WHERE Code=@0)
 INSERT INTO {databaseOwner}{objectQualifier}Connect_LPM_Locales (Code) VALUES (@0);", code);
          }
          res = context.ExecuteSingleOrDefault<Locale>(System.Data.CommandType.Text, @"SELECT * FROM {databaseOwner}{objectQualifier}Connect_LPM_Locales WHERE Code=@0;", code);
        }
        return res;
      }
    }
    public IEnumerable<Locale> GetLocaleChain(string code)
    {
      using (var context = DataContext.Instance())
      {
        return context.ExecuteQuery<Locale>(System.Data.CommandType.Text, @"SELECT * FROM {databaseOwner}{objectQualifier}Connect_LPM_Locales WHERE Code=@0 OR Code=LEFT(@0,2) ORDER BY LEN(Code)", code);
      }
    }

  }
  public partial interface ILocaleRepository
  {
    Locale GetOrCreateLocale(string code);
    IEnumerable<Locale> GetLocaleChain(string code);
  }
}

