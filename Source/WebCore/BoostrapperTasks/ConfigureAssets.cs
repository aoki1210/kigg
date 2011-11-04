using System;

namespace Kigg.Web.BoostrapperTasks
{
    using System.Linq;

    using MvcExtensions;
    using Telerik.Web.Mvc;

    using Configuration;
    using Infrastructure;
    using WebAsset = Infrastructure.WebAsset;

    public class ConfigureAssets : BootstrapperTask
    {
        private readonly Settings settings;
        public ConfigureAssets(Settings settings)
        {
            Check.Argument.IsNotNull(settings, "settings");

            this.settings = settings;
        }

        public override TaskContinuation Execute()
        {
            //WebAssetDefaultSettings.UseTelerikContentDeliveryNetwork = true;
            WebAssetDefaultSettings.Combined = true;
            WebAssetDefaultSettings.ScriptFilesPath = settings.Asset.ScriptFilesPath;
            WebAssetDefaultSettings.StyleSheetFilesPath = settings.Asset.StyleSheetFilesPath;
            WebAssetDefaultSettings.Compress = settings.Asset.Compress;

            var remoteScripts = settings.Asset.Assets
                                        .Where(a => a.Type == AssetType.RemoteJavaScript)
                                        .GroupBy(a => a.Group);

            var localScripts = settings.Asset.Assets
                                        .Where(a => a.Type == AssetType.LocalJavaScript)
                                        .GroupBy(a => a.Group);

            remoteScripts.ForEach(RegisterScripts);

            localScripts.ForEach(RegisterScripts);

            //SharedWebAssets.StyleSheets(
            //                            group => group.AddGroup(
            //                                        "appStyles",
            //                                        styles =>
            //                                            styles.Add("site.css")
            //                                                  .Add("openid.css")
            //                                                  .Add("form.css")
            //                                                  .Add("telerik.common.css")
            //                                                  .Add("telerik.forest.css")));

            //SharedWebAssets.Scripts(
            //                            group => group.AddGroup(
            //                                        "publicScripts",
            //                                        scripts =>
            //                                            scripts.Add("jquery.validate.js")
            //                                                   .Add("jquery.form.js")
            //                                                   .Add("jquery.color.js")
            //                                                   .Add("jquery.watermark.js")
            //                                                   .Add("jquery.openid.js")
            //                                                   .Add("createShortUrl.js")
            //                                                   .Add("profile.js")));

            //SharedWebAssets.Scripts(
            //                            group => group.AddGroup(
            //                                        "controlPanelScripts",
            //                                        scripts =>
            //                                            scripts.Add("administrativeItem.js")
            //                                                   .Add("bannedIpAddress.js")
            //                                                   .Add("bannedDomain.js")
            //                                                   .Add("reservedAlias.js")
            //                                                   .Add("badWord.js")
            //                                                   .Add("url.js")
            //                                                   .Add("user.js")));

            return TaskContinuation.Continue;
        }

        private static void RegisterScripts(IGrouping<string, WebAsset> scriptsGroup)
        {
            SharedWebAssets.Scripts(grp =>
                                    grp.AddGroup(scriptsGroup.Key,
                                                 scripts =>
                                                 scriptsGroup.ForEach(s => scripts.Add(s.Url))
                                        ));
        }
    }
}