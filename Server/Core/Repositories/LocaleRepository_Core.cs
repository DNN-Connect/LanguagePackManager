using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.Locales;

namespace Connect.LanguagePackManager.Core.Repositories
{

	public partial class LocaleRepository : ServiceLocator<ILocaleRepository, LocaleRepository>, ILocaleRepository
 {
        protected override Func<ILocaleRepository> GetFactory()
        {
            return () => new LocaleRepository();
        }
        public IEnumerable<Locale> GetLocales()
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Locale>();
                return rep.Get();
            }
        }
        public Locale GetLocale(int localeId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Locale>();
                return rep.GetById(localeId);
            }
        }
        public Locale AddLocale(Locale locale)
        {
            Requires.NotNull(locale);
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Locale>();
                rep.Insert(locale);
            }
            return locale;
        }
        public void DeleteLocale(Locale locale)
        {
            Requires.NotNull(locale);
            Requires.PropertyNotNegative(locale, "LocaleId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Locale>();
                rep.Delete(locale);
            }
        }
        public void DeleteLocale(int localeId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Locale>();
                rep.Delete("WHERE LocaleId = @0", localeId);
            }
        }
        public void UpdateLocale(Locale locale)
        {
            Requires.NotNull(locale);
            Requires.PropertyNotNegative(locale, "LocaleId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Locale>();
                rep.Update(locale);
            }
        } 
    }
    public partial interface ILocaleRepository
    {
        IEnumerable<Locale> GetLocales();
        Locale GetLocale(int localeId);
        Locale AddLocale(Locale locale);
        void DeleteLocale(Locale locale);
        void DeleteLocale(int localeId);
        void UpdateLocale(Locale locale);
    }
}

