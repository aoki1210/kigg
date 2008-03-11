<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Kigg.StoryDetailView"%>
<%@ Import Namespace="Kigg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <% var story = ((StoryDetailData)ViewData).Story; %>
    <% if (story == null) %>
    <% { %>
        <span class="pageMessage">The story does not exists.</span>
    <% } %>
    <% else %>
    <% { %>
        <table class="story">
            <tbody>
                <tr>
                    <td class="kigg">
                        <div class="count">
                            <span id="kiggCount"><%=story.VoteCount.ToString()%></span>
                            <br />
                            kiggs
                        </div>
                        <div id="kiggIt" class="it" style="display:<%= ((story.HasVoted) ? "none" : string.Empty) %>">
                            <a href="javascript:void(0)" onclick="javascript:Story.kigg(<%=story.ID %>, <%=story.VoteCount %>, 'kiggCount', 'kiggIt', 'kigged', 'kigging')">kigg it</a>
                        </div>
                        <div id="kigged" class="ed" style="display:<%= ((story.HasVoted) ? string.Empty : "none") %>">
                            kigged
                        </div>
                        <div id="kigging" class="ing" style="display:none">
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
                                            <%= "published " %>
                                            <span class="time"><%= story.PublishedAgo %></span> ago,
                                            <% }%>
                                            posted by
                                            <a href="<%= Url.Action("PostedBy", "Story", new { name = story.PostedBy.Name.UrlEncode(), page = 1 })%>">
                                            <img alt="" src="http://www.gravatar.com/avatar.php?gravatar_id=<%= story.PostedBy.GravatarID %>&size=15" class="gravatar" />
                                            <%= Server.HtmlEncode(story.PostedBy.Name) %>
                                            </a>
                                            <span class="time"><%= story.PostedAgo %></span> ago
                                        </div>
                                        <div class="description">
                                            <%= Server.HtmlEncode(story.Description) %>
                                        </div>
                                        <div class="summary">
                                            <span class="category">category:</span>  <%= Html.ActionLink<StoryController>(c => c.Category(story.Category.UrlEncode(), 1), story.Category) %>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <span class="tags">
                            <span class="text">tags:</span>
                            <% for(int i = 0; i < story.Tags.Length; i++) %>
                            <% { %>
                            <%     string tag = story.Tags[i]; %>
                            <%     if (i > 0) %>
                            <%     { %>
                            <%=         ", " %>
                            <%     } %>
                            <%=     Html.ActionLink<StoryController>(c => c.Tag(tag.UrlEncode(), 1), Server.HtmlEncode(tag))%>
                            <% } %>
                        </span>
                    </td>
                </tr>
            </tbody>
        </table>
        <div>
            <div class="pageTitle">
                Kigged By
            </div>
            <div>
                <div class="kiggedBy">
                    <%  foreach(UserItem votedBy in story.VotedBy) %>
                    <%  { %>
                            <span class="user">
                                <a href="<%= Url.Action("PostedBy", "Story", new { name = votedBy.Name.UrlEncode(), page = 1 })%>">
                                    <img alt="" src="http://www.gravatar.com/avatar.php?gravatar_id=<%= votedBy.GravatarID %>&size=15" class="gravatar" />
                                    <%= Server.HtmlEncode(votedBy.Name) %>
                                </a>
                            </span>
                    <%  } %>
                </div>
            </div>
        </div>
        <div class="divider"></div>
        <div>
            <div class="pageTitle">
                <a name="comments">Comments</a>
            </div>
            <div>
                <div>
                    <% if (story.Comments.Length == 0) %>
                    <% { %>
                        <span class="pageMessage">Be the first one to comment.</span>
                    <% } %>
                    <% else %>
                    <% { %>
                    <%  bool isOdd = true; %>
                    <%  foreach(CommentItem comment in story.Comments) %>
                    <%  { %>
                    <%   string className = (isOdd) ? "odd" : "even"; %>
                    <%   isOdd = !isOdd;%>
                        <div class="comment <%= className %>">
                            <div>
                                <%= Server.HtmlEncode(comment.Content) %>
                            </div>
                            <div class="postedBy">
                                posted by
                                <span class="user">
                                    <a href="<%= Url.Action("PostedBy", "Story", new { name = comment.PostedBy.Name.UrlEncode(), page = 1 })%>">
                                        <img alt="" src="http://www.gravatar.com/avatar.php?gravatar_id=<%= comment.PostedBy.GravatarID %>&size=15" class="gravatar" />
                                        <%= Server.HtmlEncode(comment.PostedBy.Name) %>
                                    </a>
                                </span>
                                <span class="time"><%= comment.PostedAgo %></span> ago
                            </div>
                        </div>
                    <%  } %>
                    <% } %>
                </div>
            </div>
            <div class="divider"></div>
            <div style="width:520px">
                <div class="form">
                    <h2>Post Your Comment</h2>
                    <p>
                        <textarea id="txtComment" cols="20" rows="8" class="largeTextarea"></textarea>
                    </p>
                    <div id="commentMessage" style="display:none;padding-top:2px;padding-bottom:2px"></div>
                    <p>
                        <input id="btnSubmitComment" type="button" class="button" value="Submit" onclick="javascript:Story.comment(<%= story.ID.ToString() %>)"/>
                    </p>
                </div>
            </div>
        </div>
    <% } %>
</asp:Content>