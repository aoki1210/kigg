<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" AutoEventWireup="true" CodeBehind="Tag.aspx.cs" Inherits="Kigg.StoryListByTagView"%>
<%@ Register src="../Shared/StoryListView.ascx" tagname="StoryListView" tagprefix="uc" %>
<%@ Import Namespace="Kigg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <% var viewData = (StoryListByTagData)ViewData; %>
    <div class="pageTitle">
        Browse by Tag : <%= Server.HtmlEncode(viewData.Tag) %>
    </div>
    <% if ((viewData.Stories != null) && (viewData.Stories.Length > 0)) %>
    <%{%>
        <uc:StoryListView runat="server"></uc:StoryListView>
    <%}%>
    <%else%>
    <%{%>
        <span class="pageMessage">No story exists with this tag.</span>
    <%}%>
</asp:Content>