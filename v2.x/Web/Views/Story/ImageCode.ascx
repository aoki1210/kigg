<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageCode.ascx.cs" Inherits="Kigg.Web.ImageCode" %>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        GenerateScripts();
    }

    private void GenerateScripts()
    {
        jQueryScriptManager scriptManager = jQueryScriptManager.Current;

        scriptManager.RegisterOnReady("ImageCode.set_promoteText('{0}');".FormatWith(ViewData.Model.PromoteText));
        scriptManager.RegisterOnReady("ImageCode.init();");
        scriptManager.RegisterOnDispose("ImageCode.dispose();");
    }

</script>

<% string rootUrl = ViewData.Model.RootUrl; %>
<div id="imageCode">
    <% string kiggUrl = "{0}{1}".FormatWith(rootUrl, Url.RouteUrl("Detail", new { name = ViewData.Model.Story.UniqueName })); %>
    <% string originalUrl = ViewData.Model.Story.Url; %>
    <% string imageUrl = "{0}/{1}".FormatWith(rootUrl, "image.axd"); %>
    <%= Html.Hidden("hidKiggUrl", kiggUrl)%>
    <%= Html.Hidden("hidOriginalUrl", originalUrl)%>
    <%= Html.Hidden("hidImageUrl", imageUrl)%>
    <div>
        <p>
            <label for="txtBorderColor">Border color:</label>
            <input id="txtBorderColor" type="text" maxlength="6"/>
            <input id="hidBorderColor" type="hidden" value="<%= ImageHandler.DefaultBorderColor %>"/>
            <span id="spnBorderColor" class="color"></span>
        </p>
        <p>
            <label for="txtTextBackColor"><%= ViewData.Model.PromoteText %> Backcolor:</label>
            <input id="txtTextBackColor" type="text" maxlength="6"/>
            <input id="hidTextBackColor" type="hidden" value="<%= ImageHandler.DefaultTextBackColor %>"/>
            <span id="spnTextBackColor" class="color"></span>
        </p>
        <p>
            <label for="txtTextForeColor"><%= ViewData.Model.PromoteText %> Forecolor:</label>
            <input id="txtTextForeColor" type="text" maxlength="6"/>
            <input id="hidTextForeColor" type="hidden" value="<%= ImageHandler.DefaultTextForeColor %>"/>
            <span id="spnTextForeColor" class="color"></span>
        </p>
        <p>
            <label for="txtCountBackColor">Counter Backcolor:</label>
            <input id="txtCountBackColor" type="text" maxlength="6"/>
            <input id="hidCountBackColor" type="hidden" value="<%= ImageHandler.DefaultCountBackColor %>"/>
            <span id="spnCountBackColor" class="color"></span>
        </p>
        <p>
            <label for="txtCountForeColor">Counter Forecolor:</label>
            <input id="txtCountForeColor" type="text" maxlength="6"/>
            <input id="hidCountForeColor" type="hidden" value="<%= ImageHandler.DefaultCountForeColor %>"/>
            <span id="spnCountForeColor" class="color"></span>
        </p>
    </div>
    <div>
        <p>
            <img id="imgPreview" alt="Preview" class="smoothImage" src="" onload="javascript:SmoothImage.show(this)"/>
        </p>
        <p>
            <textarea id="txtImageCode" cols="20" rows="4" readonly="readonly"></textarea>
        </p>
        <p>
            <a id="lnkUpdateCode" class="actionLink" href="javascript:void(0)">update</a>
            <a id="lnkResetCode" class="actionLink" href="javascript:void(0)">reset</a>
        </p>
    </div>
</div>