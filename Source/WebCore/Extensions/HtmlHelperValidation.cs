namespace Kigg.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class HtmlHelperValidation
    {
        public static MvcHtmlString TextBoxFor<T,TProperty>(this HtmlHelper<T> htmlHelper, Expression<Func<T,TProperty>> expression, string id = null, string cssClass= null)
        {
            return htmlHelper.TextBoxFor(expression, new Dictionary<string, object> {{"id", id}, {"class", cssClass}});
        }

        public static MvcHtmlString PasswordFor<T, TProperty>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TProperty>> expression, string id = null, string cssClass = null)
        {
            return htmlHelper.PasswordFor(expression, new Dictionary<string, object> { { "id", id }, { "class", cssClass } });
        }

        public static MvcHtmlString ValidationMessageFor<T,TProperty>(this HtmlHelper<T> htmlHelper, Expression<Func<T,TProperty>> expression, string cssClass= null)
        {
            return htmlHelper.ValidationMessageFor(expression, null,
                                                   new Dictionary<string, object> {{"class", cssClass}});
        }
    }
}
