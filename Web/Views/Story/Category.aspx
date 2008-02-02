<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="Kigg.StoryListByCategoryView"%>
<%@ Register src="../Shared/StoryListView.ascx" tagname="StoryListView" tagprefix="uc" %>
<%@ Import Namespace="Kigg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <% StoryListByCategoryData viewData = (StoryListByCategoryData)ViewData; %>
    <div class="pageTitle">
        <%= Server.HtmlEncode(viewData.Category) %>
    </div>
    <% if ((viewData.Stories != null) && (viewData.Stories.Length > 0)) %>
    <%{%>
        <uc:StoryListView runat="server"></uc:StoryListView>
    <%}%>
    <%else%>
    <%{%>
        <span class="pageMessage">No story exists in this category.</span>
    <%}%>
</asp:Content>