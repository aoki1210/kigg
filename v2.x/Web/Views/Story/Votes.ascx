<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Votes.ascx.cs" Inherits="Kigg.Web.Votes" %>
<ul id="voteList">
<% foreach (IVote vote in ViewData.Model) %>
<% {%>
<%      string promotedByName = vote.ByUser.UserName; %>
        <li>
            <a href="<%= Url.RouteUrl("User", new { id = vote.ByUser.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }) %>">
                <img alt="<%= Html.AttributeEncode(promotedByName) %>" src="<%= Html.AttributeEncode(vote.ByUser.GravatarUrl(24)) %>" class="smoothImage" onload="javascript:SmoothImage.show(this)"/>
                <%= Html.Encode(promotedByName)%>
            </a>
        </li>
<% }%>
</ul>