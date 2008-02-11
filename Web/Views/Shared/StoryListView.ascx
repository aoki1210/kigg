<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StoryListView.ascx.cs" Inherits="Kigg.StoryListView" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Kigg" %>
<% BaseStoryListData viewData = ViewData; %>
<% bool isOdd = true; %>
<% foreach (StoryListItem story in viewData.Stories) %>
<% { %>
<%   string className = (isOdd) ? "odd" : "even"; %>
<%   isOdd = !isOdd;%>
<%   string kiggCountId = "kiggCount" + story.ID.ToString();%>
<%   string kiggItId = "kiggIt" + story.ID.ToString();%>
<%   string kiggedId = "kigged" + story.ID.ToString();%>
<%   string kiggingId = "kigging" + story.ID.ToString();%>
    <table class="story <%= className %>">
        <tbody>
            <tr>
                <td class="kigg">
                    <div class="count">
                        <span id="<%=kiggCountId %>"><%=story.VoteCount.ToString()%></span>
                        <br />
                        kiggs
                    </div>
                    <div id="<%=kiggItId %>" class="it" style="display:<%= ((story.HasVoted) ? "none" : "''") %>">
                        <a href="javascript:void(0)" onclick="javascript:Story.kigg(<%=story.ID %>, <%=story.VoteCount %>, '<%=kiggCountId %>', '<%=kiggItId%>', '<%=kiggedId%>', '<%=kiggingId%>')">kigg it</a>
                    </div>
                    <div id="<%=kiggedId%>" class="ed" style="display:<%= ((story.HasVoted) ? "''" : "none") %>">
                        kigged
                    </div>
                    <div id="<%=kiggingId%>" class="ing" style="display:none">
                        wait...
                    </div>
                </td>
                <td class="content">
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    <div class="title">
                                        <a href="<%= story.Url %>" target="_blank"><%= Server.HtmlEncode(story.Title) %></a>
                                    </div>
                                    <div class="timestamp">
                                        <% if (story.IsPublished) %>
                                        <% {%>
                                        <%= "published "%>
                                        <span class="time"><%= story.PublishedAgo%></span> ago,
                                        <% }%>
                                        posted by
                                        <a href="<%= Url.Action(new { controller = "Story", action = "PostedBy", name = story.PostedBy.Name.UrlEncode(), page = 1 })%>">
                                        <img alt="" src="http://www.gravatar.com/avatar.php?gravatar_id=<%= story.PostedBy.GravatarID %>&size=15" class="gravatar" />
                                        <%= Server.HtmlEncode(story.PostedBy.Name) %>
                                        </a>
                                        <span class="time"><%= story.PostedAgo%></span> ago
                                    </div>
                                    <div class="description">
                                        <%= Server.HtmlEncode(story.StrippedDescription)%>
                                        <%= Html.ActionLink("(more)", new { controller = "Story", action = "Detail", id = story.ID })%>
                                    </div>
                                    <div class="summary">
                                        <span class="category">category:</span> <%= Html.ActionLink(Server.HtmlEncode(story.Category), new { controller = "Story", action = "Category", name = story.Category.UrlEncode(), page = 1 })%> |
                                        <% string commentText = string.Empty; %>
                                        <% if (story.CommentCount == 0) %>
                                        <% { %>
                                        <%    commentText = "add a comment";%>
                                        <% } %>
                                        <% else %>
                                        <% { %>
                                        <%    commentText = story.CommentCount.ToString() + ((story.CommentCount == 1) ? " comment" : " comments");%>
                                        <% } %>
                                        <a href="<%= Url.Action(new { controller = "Story", action = "Detail", id = story.ID })%>#comments" class="addComment"><%= commentText%></a>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <span class="tags">
                        <span class="text">tags:</span>
                        <% for (int i = 0; i < story.Tags.Length; i++) %>
                        <% { %>
                        <%     string tag = story.Tags[i]; %>
                        <%     if (i > 0) %>
                        <%     { %>
                        <%=         ", "%>
                        <%     } %>
                        <%=     Html.ActionLink(Server.HtmlEncode(tag), new { controller = "Story", action = "Tag", name = tag.UrlEncode(), page = 1 })%>
                        <% } %>
                    </span>
                </td>
            </tr>
        </tbody>
    </table>
<% } %>
<div class="pager">
<script runat="server">

const int SliderSize = 7;

object GetActionLinkArguments(int page)
{
    IDictionary<string, object> values = ViewContext.RouteData.Values;

    values["page"] = page;

    //By Default we allow the top right search form to post
    foreach (string key in Request.Form)
    {
        values[key] = Request.Form[key];
    }

    //The user might also navigate by specifying the searchText in querystring
    foreach (string key in Request.QueryString)
    {
        if (!values.ContainsKey(key))
        {
            values[key] = Request.QueryString[key];
        }
    }

    return values;
}

void CalculateStartAndEnd(BaseStoryListData viewData, out int start, out int end)
{
    int pageCount = viewData.PageCount;

    if (pageCount <= SliderSize)
    {
        start = 1;
        end = pageCount;
    }
    else
    {
        int middle = (int)Math.Ceiling(SliderSize / 2d) - 1;
        int below = (viewData.CurrentPage - middle);
        int above = (viewData.CurrentPage + middle);

        if (below < 4)
        {
            above = SliderSize;
            below = 1;
        }
        else if (above > (pageCount - 4))
        {
            above = viewData.PageCount;
            below = (pageCount - SliderSize);
        }

        start = below;
        end = above;
    }
}
</script>

    <% if (viewData.StoryCount > viewData.StoryPerPage) %>
    <%{%>
        <% if (viewData.CurrentPage > 1) %>
        <% { %>
            <%= Html.ActionLink("Previous", GetActionLinkArguments(viewData.CurrentPage - 1))%>
        <% } %>
        <% else %>
        <% { %>
            <span class="disabled">Previous</span>
        <% } %>

        <% int start = 0; %>
        <% int end = 0; %>
        <% CalculateStartAndEnd(viewData, out start, out end); %>

        <% if (start > 3) %>
        <% { %>
            <%= Html.ActionLink("1", GetActionLinkArguments(1))%>
            <%= Html.ActionLink("2", GetActionLinkArguments(2))%>
            ...
        <% } %>

        <% for (int i = start; i <= end; i++) %>
        <% { %>
        <%  if (i == viewData.CurrentPage) %>
        <%  { %>
                <span class="current"><%= i%></span>
        <%  } %>
        <%  else %>
        <%  { %>
                <%= Html.ActionLink(i.ToString(), GetActionLinkArguments(i))%>
        <%  } %>
        <% } %>

        <% if (end < (viewData.PageCount - 3)) %>
        <% { %>
                ...
                <%= Html.ActionLink((viewData.PageCount - 1).ToString(), GetActionLinkArguments((viewData.PageCount - 1)))%>
                <%= Html.ActionLink(viewData.PageCount.ToString(), GetActionLinkArguments((viewData.PageCount)))%>
        <% } %>

        <% if (viewData.CurrentPage < viewData.PageCount) %>
        <% { %>
            <%= Html.ActionLink("Next", GetActionLinkArguments((viewData.CurrentPage + 1)))%>
        <% } %>
        <% else %>
        <% { %>
            <span class="disabled">Next</span>
        <% } %>
    <% } %>
</div>