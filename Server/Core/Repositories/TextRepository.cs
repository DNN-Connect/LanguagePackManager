using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.Texts;

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
    }
    public partial interface ITextRepository
    {
        TextBase AddText(int packageVersionId, int resourceFileId, string key, string value);
    }
}

