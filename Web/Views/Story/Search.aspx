<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Kigg.StoryListBySearchView"%>
<%@ Register src="../Shared/StoryListView.ascx" tagname="StoryListView" tagprefix="uc" %>
<%@ Import Namespace="Kigg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <% var viewData = (StoryListBySearchData)ViewData; %>
    <div class="pageTitle">
        Search Result : <%= Server.HtmlEncode(viewData.SearchQuery) %>
    </div>
    <% if ((viewData.Stories != null) && (viewData.Stories.Length > 0)) %>
    <%{%>
        <uc:StoryListView runat="server"></uc:StoryListView>
    <%}%>
    <%else%>
    <%{%>
        <span class="pageMessage">No story found for this search criteria.</span>
    <%}%>
</asp:Content>