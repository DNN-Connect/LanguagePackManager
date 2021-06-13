using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Routing;
using System.Web.Mvc;
using System.Web.Routing;

namespace Connect.LanguagePackManager.Presentation.Common
{
    public class LanguagePackManagerMvcController : DnnController
    {

        private ContextHelper _LanguagePackManagerModuleContext;
        public ContextHelper LanguagePackManagerModuleContext
        {
            get { return _LanguagePackManagerModuleContext ?? (_LanguagePackManagerModuleContext = new ContextHelper(this)); }
        }

    }
}