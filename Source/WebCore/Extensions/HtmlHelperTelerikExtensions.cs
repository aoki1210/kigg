namespace Kigg.Web
{
    using System.Web.Mvc;

    using Telerik.Web.Mvc.UI;

    public static class HtmlHelperExtensions
    {
        public static ScriptRegistrarBuilder Scripts(this HtmlHelper htmlHelper, string[] sharedGroups = null, bool enablejQuery = false)
        {
            var jsBuilder = htmlHelper.Telerik()
                .ScriptRegistrar()
                .jQuery(enablejQuery)
                .jQueryValidation(enablejQuery);
            
            jsBuilder = (!sharedGroups.IsNullOrEmpty())
                       ? jsBuilder.Scripts(scripts => sharedGroups.ForEach(g => scripts.AddSharedGroup(g)))
                       : jsBuilder;
            
            return jsBuilder;
        }

        public static StyleSheetRegistrarBuilder StyleSheets(this HtmlHelper htmlHelper, string[] sharedGroups)
        {
            var jsBuilder = htmlHelper.Telerik()
                .StyleSheetRegistrar();

            return (!sharedGroups.IsNullOrEmpty())
                       ? jsBuilder.StyleSheets(styles => sharedGroups.ForEach(g => styles.AddSharedGroup(g)))
                       : jsBuilder;
        }
    }
}
