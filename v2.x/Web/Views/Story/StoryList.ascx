<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StoryList.ascx.cs" Inherits="Kigg.Web.StoryList" %>
<%= Html.PageHeader(ViewData.Model.Subtitle, ViewData.Model.RssUrl, ViewData.Model.AtomUrl)%>
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
    <%= Html.StoryListPager() %>
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