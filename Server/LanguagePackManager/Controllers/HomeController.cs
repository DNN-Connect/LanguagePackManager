using System.Web.Mvc;
using Connect.LanguagePackManager.Presentation.Common;

namespace Connect.LanguagePackManager.Presentation.Controllers
{
    public class HomeController : LanguagePackManagerMvcController
    {
        [HttpGet]
        [LpmAuthorize(SecurityLevel = SecurityAccessLevel.View)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
