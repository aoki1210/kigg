namespace Kigg.Web.Controllers
{
    using System.Web.Mvc;

    public class GeneralController : KiggControllerBase
    {
        [HttpGet]
        public ActionResult JavaScriptConstants()
        {
            var constants = new
            {
                ActionUrls = new
                 {
                     Home = Url.Home(),
                     Logout = Url.Logout()
                 }
            };

            return Json(constants, JsonRequestBehavior.AllowGet);
        }
    }
}
