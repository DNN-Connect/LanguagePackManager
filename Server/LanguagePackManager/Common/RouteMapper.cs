using DotNetNuke.Web.Api;

namespace Connect.LanguagePackManager.Presentation.Common
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("Connect/LanguagePackManager", "LanguagePackManagerMap1", "{controller}/{action}", null, null, new[] { "Connect.LanguagePackManager.Presentation.Api" });
            mapRouteManager.MapHttpRoute("Connect/LanguagePackManager", "LanguagePackManagerMap2", "{controller}/{action}/{id}", null, new { id = "-?\\d+" }, new[] { "Connect.LanguagePackManager.Presentation.Api" });
        }
    }
}