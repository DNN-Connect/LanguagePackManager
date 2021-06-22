using System.Web.Mvc;
using Connect.LanguagePackManager.Presentation.Common;

namespace Connect.LanguagePackManager.Presentation.Controllers
{
    public class HomeController : LanguagePackManagerMvcController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
