<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserStoryList.ascx.cs" Inherits="Kigg.Web.UserStoryList" %>
<div style="float:left;width:100%;text-align:right">
    <%= Html.SyndicationIcons(ViewData.Model.RssUrl, ViewData.Model.AtomUrl) %>
</div>
<%if (!ViewData.Model.Stories.IsNullOrEmpty()) %>
<%{ %>
    <% bool isOdd = true; %>
    <% foreach (IStory story in ViewData.Model.Stories) %>
    <% { %>
    <%      string className = (isOdd) ? "odd" : "even"; %>
            <table id="t-<%= Html.AttributeEncode(story.Id.Shrink()) %>" class="story <%= className %> hentry">
                <tbody>
                    <tr>
                        <% Html.RenderPartial("Story", new StoryItemViewData { Story = story, User = ViewData.Model.CurrentUser, PromoteText = ViewData.Model.PromoteText, DemoteText = ViewData.Model.DemoteText, CountText = ViewData.Model.CountText, SocialServices = ViewData.Model.SocialServices, DetailMode = false }); %>
                    </tr>
                </tbody>
            </table>
    <%   isOdd = !isOdd;%>
    <% } %>
    <%= Html.UserStoryListPager(ViewData.Model.SelectedTab)%>
    <% if (ViewData.Model.CanCurrentUserModerate)
       {
           Html.RenderPartial("StoryEditorBox");
       }
    %>
<%} %>
<%else%>
<%{%>
    <span class="pageMessage"><%= ViewData.Model.NoStoryExistMessage%></span>
<%}%>