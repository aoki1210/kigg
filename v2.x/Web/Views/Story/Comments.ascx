﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Comments.ascx.cs" Inherits="Kigg.Web.Comments" %>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        GenerateScripts();
    }

    private void GenerateScripts()
    {
        jQueryScriptManager scriptManager = jQueryScriptManager.Current;

        scriptManager.RegisterOnReady("Comment.set_captchaEnabled({0});".FormatWith(ViewData.Model.CaptchaEnabled.ToString().ToLowerInvariant()));
        scriptManager.RegisterOnReady("Comment.init();");
        scriptManager.RegisterOnDispose("Comment.dispose();");
    }

</script>
<%
    IStory story = ViewData.Model.Story;
    string message = story.HasComments() ? "{0} {1} posted.".FormatWith(story.CommentCount, (story.CommentCount > 1 ? "comments" : "comment")) : "No comments yet, be the first one to post comment.";
%>
<div class="commentMessage"><h2><%= message%></h2></div>
<div id="commentList">
    <%if (story.HasComments()) %>
    <%{%>
        <ul>
            <%int i = 0; %>
            <%foreach (IComment comment in story.Comments) %>
            <%{%>
                <% i += 1;%>
                <% string commentId = comment.Id.Shrink(); %>
                <% string userUrl = Url.RouteUrl("User", new { id = comment.ByUser.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }); %>
                <% bool isOwner = story.IsPostedBy(comment.ByUser); %>
                <% bool canModarate = ((ViewData.Model.CurrentUser != null) && ViewData.Model.CurrentUser.CanModerate()); %>
                <li id="li-<%= Html.AttributeEncode(commentId)%>">
                    <div class="hreview">
                        <div class="hide">
                            <span class="type">url</span>
                            <div class="item">
                                <a href="<%= Html.AttributeEncode(story.Url) %>" class="fn url"><%= Html.Encode(story.Title) %></a>
                            </div>
                            <abbr class="dtreviewed" title="<%=comment.PostedAt.ToString("yyyy-MM-ddThh:mm:ssZ")%>"><%=comment.PostedAt.ToString("F")%> GMT</abbr>
                        </div>
                    </div>
                    <p class="meta <%= isOwner ? "metaOwner" : "metaVisitor" %>">
                        <span class="no">
                            <a name="c-<%=Html.AttributeEncode(commentId)%>" title="Permalink" class="permalink" rel="bookmark" href="#c-<%=Html.AttributeEncode(commentId)%>">#<%=i.ToString()%></a>
                        </span>
                        <span class="postedBy reviewer vcard">
                            <a href="<%= Html.AttributeEncode(userUrl) %>"><img class="photo" alt="<%= Html.AttributeEncode(comment.ByUser.UserName) %>" src="<%= Html.AttributeEncode(comment.ByUser.GravatarUrl(15)) %>"/><span class="fn"><%= Html.Encode(comment.ByUser.UserName) %></span></a>
                        </span>
                        <span class="time" title="<%=comment.PostedAt.ToString("F")%> GMT"><%= comment.PostedAt.ToRelative() %></span> ago said:
                    </p>
                    <div class="body <%= isOwner ? "ownerBody" : "visitorBody" %>">
                        <div class="description">
                            <%if (canModarate) %>
                            <%{%>
                                <%= comment.HtmlBody %>
                                <div class="summary">
                                    <%string storyId = story.Id.Shrink(); %>
                                    <%if (!comment.IsOffended)%>
                                    <%{%>
                                        <a class="flagOffended actionLink" href="javascript:void(0)" onclick="Moderation.markCommentAsOffended('<%= storyId %>', '<%= commentId %>')">flag as offended</a> | 
                                    <%}%>
                                    <a class="spam actionLink" href="javascript:void(0)" onclick="Moderation.confirmSpamComment('<%= storyId %>', '<%= commentId %>')">spam</a>
                                </div>
                            <%}%>
                            <%else%>
                            <%{%>
                                <%if (comment.IsOffended) %>
                                <%{%>
                                    <em>This comment has been marked as offended.</em>
                                <%}%>
                                <%else%>
                                <%{%>
                                <%= comment.HtmlBody %>
                                <%}%>
                            <%}%>
                        </div>
                    </div>
                </li>
            <%}%>
        </ul>
    <%}%>
</div>
<% if (ViewData.Model.IsCurrentUserAuthenticated) %>
<% {%>
    <div class="form">
        <form id="frmCommentSubmit" action="<%= Url.Action("Post", "Comment") %>" method="post">
            <h3>Post your comment</h3>
            <p>
                <input type="hidden" id="hidId" name="id" value="<%= story.Id.Shrink() %>"/>
                <input type="hidden" id="hidbody" name="body"/>
                <textarea id="txtCommentBody" name="commentBody" cols="20" rows="4"></textarea>
                <span class="error"></span>
            </p>
            <div>
                <div id="commentPreview" class="livePreview"></div>
                <div><a id="lnkCommentPreview"  class="actionLink" href="javascript:void(0)">hide preview</a></div>
                <div><label class="smallLabel"><input id="chkSubscribe" name="subscribe" type="checkbox" value="true" checked="checked"/>Subscribe via email</label></div>
            </div>
            <%if(ViewData.Model.CaptchaEnabled)%>
            <%{%>
                <kigg:reCAPTCHA id="captcha" runat="server"></kigg:reCAPTCHA>
            <%}%>
            <p>
                <span id="commentMessage" class="message"></span>
            </p>
            <p>
                <input id="btnCommentSubmit" type="submit" class="largeButton" value="Submit"/>
            </p>
        </form>
    </div>
<% }%>
<% else %>
<% {%>
    <div class="commentMessage">
        To post your comment please <a id="lnkCommentLogin" class="actionLink" href="javascript:void(0)">login</a> or <a id="lnkCommentSignup" class="actionLink" href="javascript:void(0)">signup</a>
    </div>
<% }%>