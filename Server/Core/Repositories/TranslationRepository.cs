using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.Translations;

namespace Connect.LanguagePackManager.Core.Repositories
{
	public partial class TranslationRepository : ServiceLocator<ITranslationRepository, TranslationRepository>, ITranslationRepository
    {
    }
    public partial interface ITranslationRepository
    {
    }
}

