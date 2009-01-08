﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tabs.ascx.cs" Inherits="Kigg.Web.TagTabs" %>
<%
bool shouldShowPopularTab = !ViewData.Model.PopularTags.IsNullOrEmpty();
bool shouldShowMyTagsTab = !ViewData.Model.UserTags.IsNullOrEmpty();

if (shouldShowPopularTab || shouldShowMyTagsTab)
{
%>
    <div id="tagTabs" class="hide">
        <ul>
            <% if (shouldShowPopularTab) %>
            <% { %>
                    <li class="sidebar-tabs-nav-item"><a href="#popularTags">Popular Tags</a></li>
            <% } %>
            <% if (shouldShowMyTagsTab) %>
            <% { %>
                    <li class="sidebar-tabs-nav-item"><a href="#myTags">My Tags</a></li>
            <% } %>
        </ul>
        <% if (shouldShowPopularTab) %>
        <% { %>
            <div id="popularTags" class="tagCloud">
                <% Html.RenderPartial("Cloud", ViewData.Model.PopularTags); %>
            </div>
        <% } %>
        <% if (shouldShowMyTagsTab) %>
        <% { %>
            <div id="myTags" class="tagCloud">
                <% Html.RenderPartial("Cloud", ViewData.Model.UserTags); %>
            </div>
        <% } %>
    </div>
<%
}
%>