<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopTabs.ascx.cs" Inherits="Kigg.Web.TopUserTabs" %>
<%
bool shouldShowMoverTab = (!ViewData.Model.TopMovers.IsNullOrEmpty());
bool shouldShowLeaderTab = (!ViewData.Model.TopLeaders.IsNullOrEmpty());

if (shouldShowMoverTab || shouldShowLeaderTab)
{
%>
    <div id="topUserTabs" class="hide">
        <ul>
            <% if (shouldShowMoverTab) %>
            <% { %>
                    <li class="sidebar-tabs-nav-item"><a href="#topMovers">Top Movers</a></li>
            <% } %>
            <% if (shouldShowLeaderTab) %>
            <% { %>
                    <li class="sidebar-tabs-nav-item"><a href="#topLeaders">Top Leaders</a></li>
            <% } %>
        </ul>
        <% if (shouldShowMoverTab) %>
        <% { %>
            <div id="topMovers" class="topUsers">
                <% Html.RenderPartial("Top", ViewData.Model.TopMovers); %>
            </div>
        <% } %>
        <% if (shouldShowLeaderTab) %>
        <% { %>
            <div id="topLeaders" class="topUsers">
                <% Html.RenderPartial("Top", ViewData.Model.TopLeaders); %>
            </div>
        <% } %>
    </div>
<%
}
%>