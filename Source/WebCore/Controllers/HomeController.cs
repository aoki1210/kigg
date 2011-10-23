namespace Kigg.Web.Controllers
{
    using System.Web.Mvc;

    public class HomeController : KiggControllerBase
    {
        public ActionResult Default()
        {
            return View();
        }
    }
}
