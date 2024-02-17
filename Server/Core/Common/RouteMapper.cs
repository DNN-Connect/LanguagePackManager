using DotNetNuke.Web.Api;

namespace Connect.LanguagePackManager.Core.Common
{
  public class RouteMapper : IServiceRouteMapper
  {
    public void RegisterRoutes(IMapRoute mapRouteManager)
    {
      mapRouteManager.MapHttpRoute("Connect/LPM", "lpm1", "{controller}/{action}", null, null, new[] { "Connect.LanguagePackManager.Core.Api" });
      mapRouteManager.MapHttpRoute("Connect/LPM", "lpm2", "{controller}/{action}/{id}", null, new { id = "-?\\d+" }, new[] { "Connect.LanguagePackManager.Core.Api" });
    }
  }
}