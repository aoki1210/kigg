using System;

namespace Kigg.Web
{
    using System.Web.Mvc;
    using Repository;

    using DomainObjects;
    using Infrastructure;

    public class SupportController : BaseController
    {
        private readonly IEmailSender _emailSender;
        private readonly IStoryRepository _storyRepository;

        public SupportController(IStoryRepository storyRepository, IEmailSender emailSender)
        {
            Check.Argument.IsNotNull(storyRepository, "storyRepository");
            Check.Argument.IsNotNull(emailSender, "emailSender");

            _storyRepository = storyRepository;
            _emailSender = emailSender;
        }

        [Compress]
        public ActionResult Faq()
        {
            return PreparedView();
        }

        [AcceptVerbs(HttpVerbs.Get), Compress]
        public ActionResult Contact()
        {
            return PreparedView();
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
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

        [Compress]
        public ActionResult About()
        {
            return PreparedView();
        }

        public ActionResult ControlPanel()
        {
            ControlPanelViewData viewData = new ControlPanelViewData();

            if (!IsCurrentUserAuthenticated || !CurrentUser.CanModerate())
            {
                viewData.ErrorMessage = "You do not have the privilege to view it.";
            }
            else
            {
                viewData.IsAdministrator = CurrentUser.IsAdministrator();
                viewData.NewCount = _storyRepository.CountByNew();
                viewData.UnapprovedCount = _storyRepository.CountByUnapproved();

                DateTime currentTime = SystemTime.Now();
                DateTime minimumDate = currentTime.AddHours(-Settings.MaximumAgeOfStoryInHoursToPublish);
                DateTime maximumDate = currentTime.AddHours(-Settings.MinimumAgeOfStoryInHoursToPublish);

                viewData.PublishableCount = _storyRepository.CountByPublishable(minimumDate, maximumDate);
            }

            return View(viewData);
        }

        private ActionResult PreparedView()
        {
            return View(CreateViewData<SupportViewData>());
        }
    }
}