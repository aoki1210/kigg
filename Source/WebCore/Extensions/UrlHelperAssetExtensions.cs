namespace Kigg.Web
{
    using System.Web.Mvc;

    public static class UrlHelperAssetExtensions
    {
        /// <summary>
        /// Builds fav icon URL and returns valid one.
        /// </summary>
        /// <remarks>Icon expected to be under "~/Content/images/fav/"</remarks>
        /// <param name="instance">UrlHelper instance</param>
        /// <param name="icon">Icon file name with extension</param>
        /// <returns>Returns relative fav icon url</returns>
        public static string FavIcon(this UrlHelper instance, string icon)
        {
            Check.Argument.IsNotNull(instance, "helper");
            Check.Argument.IsNotNullOrEmpty(icon, "icon");

            return instance.Content("~/Content/images/fav/{0}".FormatWith(icon));
        }
        
        /// <summary>
        /// Builds image URL and returns valid one.
        /// </summary>
        /// <remarks>Image expected to be under "~/Content/images/"</remarks>
        /// <param name="instance">UrlHelper instance</param>
        /// <param name="image">Image file name with extension</param>
        /// <returns>Returns relative image url</returns>
        public static string Image(this UrlHelper instance, string image)
        {
            return instance.Content("~/Content/images/{0}".FormatWith(image));
        }

        public static string Logo(this UrlHelper instance, string theme = Constants.DefaultTheme, string logo = Constants.DefaultLogo)
        {
            Check.Argument.IsNotNull(instance, "helper");
            Check.Argument.IsNotNullOrEmpty(theme, "theme");
            Check.Argument.IsNotNullOrEmpty(logo, "logo");

            return instance.Content("~/Content/sitethemes/{0}/{1}".FormatWith(theme, logo));
        }
    }
}