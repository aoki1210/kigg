<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Top.ascx.cs" Inherits="Kigg.Web.TopUsers" %>
<%
if (!ViewData.Model.IsNullOrEmpty())
{
%>
    <ol>
        <% foreach (UserWithScore userScore in ViewData.Model) %>
        <% { %>
        <%      string userName = userScore.User.UserName; %>
                <li>
                    <a title="<%= Html.AttributeEncode(userName) %>" href="<%= Url.RouteUrl("User", new { id = userScore.User.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }) %>">
                        <img alt="<%= Html.AttributeEncode(userName) %>" src="<%= Html.AttributeEncode(userScore.User.GravatarUrl(24)) %>" class="smoothImage" onload="javascript:SmoothImage.show(this)"/>
                        <%= Html.Encode(userName.WrapAt(22)) %>
                    </a>
                    (<%= userScore.Score.ToString(FormatStrings.UserScore) %>)
                </li>
        <% } %>
    </ol>
<%
}
%>