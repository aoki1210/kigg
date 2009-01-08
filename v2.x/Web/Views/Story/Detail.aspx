<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Kigg.Web.StoryDetailView"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
    <script runat="server">

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            jQueryScriptManager.Current.RegisterSource(Url.Asset("js3"));

            Page.Header.Title = ViewData.Model.Title;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <% IStory story = ViewData.Model.Story; %>
    <table id="t-<%= Html.AttributeEncode(story.Id.Shrink()) %>" class="story odd">
        <tbody>
            <tr>
                <% Html.RenderPartial("Story", new StoryItemViewData { Story = story, User = ViewData.Model.CurrentUser, PromoteText = ViewData.Model.PromoteText, DemoteText = ViewData.Model.DemoteText, CountText = ViewData.Model.CountText, SocialServices = ViewData.Model.SocialServices, DetailMode = true }); %>
            </tr>
            <tr>
                <td></td>
                <td class="content">
                    <% Html.RenderPartial("ImageCode", ViewData.Model); %>
                </td>
            </tr>
            <tr>
                <td></td>
                <td class="content">
                    <div id="commentTabs" class="hide">
                        <ul>
                            <li class="ui-tabs-nav-item"><a href="#comments">Comments</a></li>
                            <li class="ui-tabs-nav-item"><a href="#votes">Promoted By</a></li>
                        </ul>
                        <div id="comments">
                            <% Html.RenderPartial("Comments", ViewData.Model); %>
                        </div>
                        <div id="votes">
                            <% Html.RenderPartial("Votes", story.Votes); %>
                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <%
        if (ViewData.Model.CanCurrentUserModerate)
        {
            Html.RenderPartial("StoryEditorBox");
        }
    %>
</asp:Content>