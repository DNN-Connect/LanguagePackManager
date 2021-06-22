using DotNetNuke.Security;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using Connect.LanguagePackManager.Presentation.Common;
using System.Web.Mvc;
using Connect.LanguagePackManager.Core.Common;

namespace Connect.LanguagePackManager.Presentation.Controllers
{
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    [DnnHandleError]
    public class SettingsController : LanguagePackManagerMvcController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Settings()
        {
            return View(LanguagePackManagerModuleContext.Settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supportsTokens"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Settings(ModuleSettings settings)
        {
            settings.SaveSettings(ModuleContext.Configuration);
            return RedirectToDefaultRoute();
        }
    }
}