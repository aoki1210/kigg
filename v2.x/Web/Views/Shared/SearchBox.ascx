<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchBox.ascx.cs" Inherits="Kigg.Web.SearchBox" %>
<%StoryListSearchViewData searchData = ViewData.Model as StoryListSearchViewData; %>
<%string queryText = null;  %>
<%if (searchData != null) queryText = searchData.Query; %>
<%using (Html.BeginForm("Search", "Story", FormMethod.Get, new { id = "frmSearch" }))%>
<%{%>
    <div class="searchBox">
        <%= Html.TextBox("q", queryText, new { id = "txtSearch", @class = "searchTextBox" })%>
    </div>
<%}%>