namespace Kigg.Web
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    
    public static class HtmlHelperExtension
    {
        private const string SharedViewThemesPath = "~/views/shared/themes";

        public static MvcHtmlString PartialSharedThemed(this HtmlHelper htmlHelper, string partialViewName, string themeName = Constants.DefaultTheme)
        {
            string viewPath = "{0}/{1}/{2}".FormatWith(SharedViewThemesPath, themeName, partialViewName);
            
            return htmlHelper.Partial(viewPath);
        }
    }
}
