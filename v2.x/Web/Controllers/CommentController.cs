namespace Kigg.Web
{
    using System;
    using System.Web.Mvc;

    using DomainObjects;
    using Infrastructure;
    using Repository;
    using Service;

    [Compress]
    public class CommentController : BaseController
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryService _storyService;

        public CommentController(IStoryRepository storyRepository, IStoryService storyService)
        {
            Check.Argument.IsNotNull(storyRepository, "storyRepository");
            Check.Argument.IsNotNull(storyService, "storyService");

            _storyRepository = storyRepository;
            _storyService = storyService;
        }

        public reCAPTCHAValidator CaptchaValidator
        {
            get;
            set;
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Post(string id, string body, bool? subscribe)
        {
            id = id.NullSafe();
            body = body.NullSafe();

            string captchaChallenge = null;
            string captchaResponse = null;
            bool captchaEnabled = !CurrentUser.ShouldHideCaptcha();

            if (captchaEnabled)
            {
                captchaChallenge = HttpContext.Request.Form[CaptchaValidator.ChallengeInputName];
                captchaResponse = HttpContext.Request.Form[CaptchaValidator.ResponseInputName];
            }

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Story identifier cannot be blank."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Invalid story identifier."),
                                                            new Validation(() => string.IsNullOrEmpty(body.NullSafe()), "Comment cannot be blank."),
                                                            new Validation(() => captchaEnabled && string.IsNullOrEmpty(captchaChallenge), "Captcha challenge cannot be blank."),
                                                            new Validation(() => captchaEnabled && string.IsNullOrEmpty(captchaResponse), "Captcha verification words cannot be blank."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "You are currently not authenticated."),
                                                            new Validation(() => captchaEnabled && !CaptchaValidator.Validate(CurrentUserIPAddress, captchaChallenge, captchaResponse), "Captcha verification words are incorrect.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(id.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Specified story does not exist." };
                    }
                    else
                    {
                        CommentCreateResult result = _storyService.Comment(
                                                                            story,
                                                                            string.Concat(Settings.RootUrl, Url.RouteUrl("Detail", new { name = story.UniqueName })),
                                                                            CurrentUser,
                                                                            body,
                                                                            subscribe ?? false,
                                                                            CurrentUserIPAddress,
                                                                            HttpContext.Request.UserAgent,
                                                                            ((HttpContext.Request.UrlReferrer != null) ? HttpContext.Request.UrlReferrer.ToString() : null),
                                                                            HttpContext.Request.ServerVariables
                                                                         );

                        viewData = string.IsNullOrEmpty(result.ErrorMessage) ? new JsonCreateViewData { isSuccessful = true } : new JsonViewData { errorMessage = result.ErrorMessage };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("posting comment") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ConfirmSpam(string storyId, string commentId)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(storyId), "Story identifier cannot be blank."),
                                                            new Validation(() => storyId.ToGuid().IsEmpty(), "Invalid story identifier."),
                                                            new Validation(() => string.IsNullOrEmpty(commentId), "Comment identifier cannot be blank."),
                                                            new Validation(() => commentId.ToGuid().IsEmpty(), "Invalid comment identifier."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "You are currently not authenticated."),
                                                            new Validation(() => !CurrentUser.CanModerate(), "You do not have the privilege to call this method.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(storyId.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Specified story does not exist." };
                    }
                    else
                    {
                        IComment comment = story.FindComment(commentId.ToGuid());

                        if (comment == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Specified comment does not exist." };
                        }
                        else
                        {
                            _storyService.Spam(comment, string.Concat(Settings.RootUrl, Url.RouteUrl("Detail", new { name = story.UniqueName })), CurrentUser);

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("approving comment as spam") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MarkAsOffended(string storyId, string commentId)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(storyId), "Story identifier cannot be blank."),
                                                            new Validation(() => storyId.ToGuid().IsEmpty(), "Invalid story identifier."),
                                                            new Validation(() => string.IsNullOrEmpty(commentId), "Comment identifier cannot be blank."),
                                                            new Validation(() => commentId.ToGuid().IsEmpty(), "Invalid comment identifier."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "You are currently not authenticated."),
                                                            new Validation(() => !CurrentUser.CanModerate(), "You do not have the privilege to call this method.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(storyId.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Specified story does not exist." };
                    }
                    else
                    {
                        IComment comment = story.FindComment(commentId.ToGuid());

                        if (comment == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Specified comment does not exist." };
                        }
                        else
                        {
                            _storyService.MarkAsOffended(comment, string.Concat(Settings.RootUrl, Url.RouteUrl("Detail", new { name = story.UniqueName })), CurrentUser);

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("marking comment as offended") };
                }
            }

            return Json(viewData);
        }
    }
}