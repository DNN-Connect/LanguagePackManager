using System.Web.Mvc;
using DotNetNuke.Web.Mvc.Helpers;
using DotNetNuke.Web.Mvc.Framework;

namespace Connect.LanguagePackManager.Presentation.Common
{
    public abstract class LanguagePackManagerWebPage : DnnWebViewPage
    {

        public ContextHelper LanguagePackManagerModuleContext { get; set; }

        public override void InitHelpers()
        {
            Ajax = new AjaxHelper<object>(ViewContext, this);
            Html = new DnnHtmlHelper<object>(ViewContext, this);
            Url = new DnnUrlHelper(ViewContext);
            Dnn = new DnnHelper<object>(ViewContext, this);
            LanguagePackManagerModuleContext = new ContextHelper(ViewContext);
        }

        public string SerializedResources()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(DotNetNuke.Services.Localization.LocalizationProvider.Instance.GetCompiledResourceFile(Dnn.PortalSettings, "/DesktopModules/MVC/Connect/LanguagePackManager/App_LocalResources/ClientResources.resx",
                    System.Threading.Thread.CurrentThread.CurrentCulture.Name));
        }

        public void RequirePermissionLevel(bool level)
        {
            LanguagePackManagerModuleContext.RequirePermissionLevel(level);
        }

        public string Locale
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            }
        }
    }
}