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
<%   string kiggCountId = "kiggCount" + story.ID;%>
<%   string kiggItId = "kiggIt" + story.ID;%>
<%   string kiggedId = "kigged" + story.ID;%>
<%   string kiggingId = "kigging" + story.ID;%>
    <table class="story <%= className %>">
        <tbody>
            <tr>
                <td class="kigg">
                    <div class="count">
                        <span id="<%=kiggCountId %>"><%=story.VoteCount.ToString()%></span>
                        <br />
                        kiggs
                    </div>
                    <div id="<%=kiggItId %>" class="it" style="display:<%= ((story.HasVoted) ? "none" : string.Empty) %>">
                        <a href="javascript:void(0)" onclick="javascript:Story.kigg(<%=story.ID %>, <%=story.VoteCount %>, '<%=kiggCountId %>', '<%=kiggItId%>', '<%=kiggedId%>', '<%=kiggingId%>')">kigg it</a>
                    </div>
                    <div id="<%=kiggedId%>" class="ed" style="display:<%= ((story.HasVoted) ? string.Empty : "none") %>">
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
                                        <a href="<%= Url.Action("PostedBy", "Story", new {name = story.PostedBy.Name.UrlEncode(), page = 1 })%>">
                                        <img alt="" src="http://www.gravatar.com/avatar.php?gravatar_id=<%= story.PostedBy.GravatarID %>&size=15" class="gravatar" />
                                        <%= Server.HtmlEncode(story.PostedBy.Name) %>
                                        </a>
                                        <span class="time"><%= story.PostedAgo%></span> ago
                                    </div>
                                    <div class="description">
                                        <%= Server.HtmlEncode(story.StrippedDescription)%>
                                        <% int storyId = story.ID; %>
                                        <%= Html.ActionLink<StoryController>(c => c.Detail(storyId), "(more)")%>
                                    </div>
                                    <div class="summary">
                                        <% string encodedCategory = story.Category.UrlEncode(); %>
                                        <span class="category">category:</span> <%= Html.ActionLink<StoryController>(c => c.Category(encodedCategory, 1), story.Category) %>
                                        <% string commentText; %>
                                        <% if (story.CommentCount == 0) %>
                                        <% { %>
                                        <%    commentText = "add a comment";%>
                                        <% } %>
                                        <% else %>
                                        <% { %>
                                        <%    commentText = story.CommentCount + ((story.CommentCount == 1) ? " comment" : " comments");%>
                                        <% } %>
                                        <a href="<%= Url.Action("Detail", "Story", new {id = story.ID })%>#comments" class="addComment"><%= commentText%></a>
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
                        <%=     Html.ActionLink<StoryController>(c => c.Tag(tag.UrlEncode(), 1), Server.HtmlEncode(tag))%>
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

string GetPagerLink(string text, int page)
{
    var values = new RouteValueDictionary();

    foreach(KeyValuePair<string, object> item in ViewContext.RouteData.Values)
    {
        if ((string.Compare(item.Key, "controller", true) != 0) && (string.Compare(item.Key, "action", true) != 0))
        {
            if (ViewContext.RouteData.Values[item.Key] != null)
            {
                values[item.Key] = item.Value;
            }
        }
    }

    values["page"] = page;

    //The user might also navigate by specifying the searchText in querystring
    foreach (string key in Request.QueryString)
    {
        if (!values.ContainsKey(key))
        {
            if (!string.IsNullOrEmpty(Request.QueryString[key]))
            {
                values[key] = Request.QueryString[key];
            }
        }
    }

    string actionName = ViewContext.RouteData.Values["action"].ToString();
    string url = Html.ActionLink(text, actionName, values);

    return url;
}

static void CalculateStartAndEnd(BaseStoryListData viewData, out int start, out int end)
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
            <%= GetPagerLink("Previous", (viewData.CurrentPage - 1))%>
        <% } %>
        <% else %>
        <% { %>
            <span class="disabled">Previous</span>
        <% } %>

        <% int start; %>
        <% int end; %>
        <% CalculateStartAndEnd(viewData, out start, out end); %>

        <% if (start > 3) %>
        <% { %>
            <%= GetPagerLink("1", 1)%>
            <%= GetPagerLink("2", 2)%>
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
                <%= GetPagerLink(i.ToString(), i)%>
        <%  } %>
        <% } %>

        <% if (end < (viewData.PageCount - 3)) %>
        <% { %>
                ...
                <%= GetPagerLink((viewData.PageCount - 1).ToString(), (viewData.PageCount - 1))%>
                <%= GetPagerLink(viewData.PageCount.ToString(), viewData.PageCount)%>
        <% } %>

        <% if (viewData.CurrentPage < viewData.PageCount) %>
        <% { %>
            <%= GetPagerLink("Next", (viewData.CurrentPage + 1))%>
        <% } %>
        <% else %>
        <% { %>
            <span class="disabled">Next</span>
        <% } %>
    <% } %>
</div>