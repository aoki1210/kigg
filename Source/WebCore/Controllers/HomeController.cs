namespace Kigg.Web.Controllers
{
    using System.Web.Mvc;

    public class HomeController : KiggControllerBase
    {
        public readonly ICookie cookie;

        public HomeController(ICookie cookie)
        {
            this.cookie = cookie;
        }

        public ActionResult Default()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ShowQueuedMessage()
        {
            string message = string.Empty;
            
            bool isError = false;
            
            var notificationMessage = cookie.GetValues(Constants.CookieNames.Notification, true);
            
            if (notificationMessage != null)
            {
                message = notificationMessage[Constants.CookieNames.Msg];

                if (!string.IsNullOrEmpty(message))
                {
                    bool.TryParse(notificationMessage[Constants.CookieNames.Err], out isError);

                    message = message.Replace("'", string.Empty).Replace("\"", string.Empty);
                }
            }

            ViewData.Add(Constants.CookieNames.Msg, message);
            ViewData.Add(Constants.CookieNames.Err, isError);
            return View("_queuedmessageBox");
        }
    }
}
