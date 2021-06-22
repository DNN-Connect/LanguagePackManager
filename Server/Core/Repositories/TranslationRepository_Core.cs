using System;
using System.Collections.Generic;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.Translations;

namespace Connect.LanguagePackManager.Core.Repositories
{

	public partial class TranslationRepository : ServiceLocator<ITranslationRepository, TranslationRepository>, ITranslationRepository
 {
        protected override Func<ITranslationRepository> GetFactory()
        {
            return () => new TranslationRepository();
        }
        public IEnumerable<Translation> GetTranslationsByLocale(int locale)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteQuery<Translation>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_Translations WHERE Locale=@0",
                    locale);
            }
        }
        public IEnumerable<Translation> GetTranslationsByText(int textId)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteQuery<Translation>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_Translations WHERE TextId=@0",
                    textId);
            }
        }
        public Translation GetTranslation(int textId, int locale)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteSingleOrDefault<Translation>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_Translations WHERE TextId=@0 AND Locale=@1",
                    textId,locale);
            }
        }
        public void AddTranslation(TranslationBase translation, int userId)
        {
            Requires.NotNull(translation);
            Requires.NotNull(translation.Locale);
            Requires.NotNull(translation.TextId);
            translation.CreatedByUserID = userId;
            translation.CreatedOnDate = DateTime.Now;
            translation.LastModifiedByUserID = userId;
            translation.LastModifiedOnDate = DateTime.Now;
            using (var context = DataContext.Instance())
            {
                context.Execute(System.Data.CommandType.Text,
                    "IF NOT EXISTS (SELECT * FROM {databaseOwner}{objectQualifier}Connect_LPM_Translations " +
                    "WHERE TextId=@0 AND Locale=@1) " +
                    "INSERT INTO {databaseOwner}{objectQualifier}Connect_LPM_Translations (TextId, Locale, TextValue, CreatedByUserID, CreatedOnDate, LastModifiedByUserID, LastModifiedOnDate) " +
                    "SELECT @0, @1, @2, @3, @4, @5, @6", translation.TextId, translation.Locale, translation.TextValue, translation.CreatedByUserID, translation.CreatedOnDate, translation.LastModifiedByUserID, translation.LastModifiedOnDate);
            }
        }
        public void DeleteTranslation(TranslationBase translation)
        {
            DeleteTranslation(translation.TextId, translation.Locale);
        }
        public void DeleteTranslation(int textId, int locale)
        {
             Requires.NotNull(locale);
             Requires.NotNull(textId);
            using (var context = DataContext.Instance())
            {
                context.Execute(System.Data.CommandType.Text,
                    "DELETE FROM {databaseOwner}{objectQualifier}Connect_LPM_Translations WHERE TextId=@0 AND Locale=@1",
                    textId,locale);
            }
        }
        public void DeleteTranslationsByLocale(int locale)
        {
            using (var context = DataContext.Instance())
            {
                context.Execute(System.Data.CommandType.Text,
                    "DELETE FROM {databaseOwner}{objectQualifier}Connect_LPM_Translations WHERE Locale=@0",
                    locale);
            }
        }
        public void DeleteTranslationsByText(int textId)
        {
            using (var context = DataContext.Instance())
            {
                context.Execute(System.Data.CommandType.Text,
                    "DELETE FROM {databaseOwner}{objectQualifier}Connect_LPM_Translations WHERE TextId=@0",
                    textId);
            }
        }
        public void UpdateTranslation(TranslationBase translation, int userId)
        {
            Requires.NotNull(translation);
            Requires.NotNull(translation.Locale);
            Requires.NotNull(translation.TextId);
            translation.LastModifiedByUserID = userId;
            translation.LastModifiedOnDate = DateTime.Now;
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<TranslationBase>();
                rep.Update("SET TextValue=@0, CreatedByUserID=@1, CreatedOnDate=@2, LastModifiedByUserID=@3, LastModifiedOnDate=@4 WHERE TextId=@5 AND Locale=@6",
                          translation.TextValue,translation.CreatedByUserID,translation.CreatedOnDate,translation.LastModifiedByUserID,translation.LastModifiedOnDate, translation.TextId,translation.Locale);
            }
        } 
 }

    public partial interface ITranslationRepository
    {
        IEnumerable<Translation> GetTranslationsByLocale(int locale);
        IEnumerable<Translation> GetTranslationsByText(int textId);
        Translation GetTranslation(int textId, int locale);
        void AddTranslation(TranslationBase translation, int userId);
        void DeleteTranslation(TranslationBase translation);
        void DeleteTranslation(int textId, int locale);
        void DeleteTranslationsByLocale(int locale);
        void DeleteTranslationsByText(int textId);
        void UpdateTranslation(TranslationBase translation, int userId);
    }
}

