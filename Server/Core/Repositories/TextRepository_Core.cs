using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.Texts;

namespace Connect.LanguagePackManager.Core.Repositories
{

	public partial class TextRepository : ServiceLocator<ITextRepository, TextRepository>, ITextRepository
 {
        protected override Func<ITextRepository> GetFactory()
        {
            return () => new TextRepository();
        }
        public IEnumerable<Text> GetTexts()
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Text>();
                return rep.Get();
            }
        }
        public IEnumerable<Text> GetTextsByPackageVersion(int packageVersionId)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteQuery<Text>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_Texts WHERE PackageVersionId=@0",
                    packageVersionId);
            }
        }
        public IEnumerable<Text> GetTextsByResourceFile(int resourceFileId)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteQuery<Text>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_Texts WHERE ResourceFileId=@0",
                    resourceFileId);
            }
        }
        public Text GetText(int textId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<Text>();
                return rep.GetById(textId);
            }
        }
        public TextBase AddText(TextBase text)
        {
            Requires.NotNull(text);
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<TextBase>();
                rep.Insert(text);
            }
            return text;
        }
        public void DeleteText(TextBase text)
        {
            Requires.NotNull(text);
            Requires.PropertyNotNegative(text, "TextId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<TextBase>();
                rep.Delete(text);
            }
        }
        public void DeleteText(int textId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<TextBase>();
                rep.Delete("WHERE TextId = @0", textId);
            }
        }
        public void UpdateText(TextBase text)
        {
            Requires.NotNull(text);
            Requires.PropertyNotNegative(text, "TextId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<TextBase>();
                rep.Update(text);
            }
        } 
    }
    public partial interface ITextRepository
    {
        IEnumerable<Text> GetTexts();
        IEnumerable<Text> GetTextsByPackageVersion(int packageVersionId);
        IEnumerable<Text> GetTextsByResourceFile(int resourceFileId);
        Text GetText(int textId);
        TextBase AddText(TextBase text);
        void DeleteText(TextBase text);
        void DeleteText(int textId);
        void UpdateText(TextBase text);
    }
}

