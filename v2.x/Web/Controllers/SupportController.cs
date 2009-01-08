namespace Kigg.Web
{
    using System.Web.Mvc;

    using Infrastructure;

    [Compress]
    public class SupportController : BaseController
    {
        private readonly IEmailSender _emailSender;

        public SupportController(IEmailSender emailSender)
        {
            Check.Argument.IsNotNull(emailSender, "emailSender");

            _emailSender = emailSender;
        }

        public ActionResult Faq()
        {
            return PreparedView();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Contact()
        {
            return PreparedView();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Contact(string email, string name, string message)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                                                                new Validation(() => string.IsNullOrEmpty(email), "Email cannot be blank."),
                                                                new Validation(() => !email.IsEmail(), "Invalid email format."),
                                                                new Validation(() => string.IsNullOrEmpty(name), "Name cannot be blank."),
                                                                new Validation(() => name.Length < 4, "Name cannot be less than 4 character."),
                                                                new Validation(() => string.IsNullOrEmpty(message), "Message cannot be blank."),
                                                                new Validation(() => message.Length < 16, "Message cannot be less than 16 character.")
                                                          );

            if (viewData == null)
            {
                _emailSender.NotifyFeedback(email, name, message);
                viewData = new JsonViewData { isSuccessful = true };
            }

            return Json(viewData);
        }

        public ActionResult About()
        {
            return PreparedView();
        }

        private ActionResult PreparedView()
        {
            return View(CreateViewData<SupportViewData>());
        }
    }
}