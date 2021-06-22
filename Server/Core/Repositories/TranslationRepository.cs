using DotNetNuke.Framework;

namespace Connect.LanguagePackManager.Core.Repositories
{
    public partial class TranslationRepository : ServiceLocator<ITranslationRepository, TranslationRepository>, ITranslationRepository
    {
    }
    public partial interface ITranslationRepository
    {
    }
}

