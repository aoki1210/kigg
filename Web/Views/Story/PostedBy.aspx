<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" AutoEventWireup="true" CodeBehind="PostedBy.aspx.cs" Inherits="Kigg.StoryListByUserView"%>
<%@ Register src="../Shared/StoryListView.ascx" tagname="StoryListView" tagprefix="uc" %>
<%@ Import Namespace="Kigg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <% StoryListByUserData viewData = (StoryListByUserData)ViewData; %>
    <div class="pageTitle">
        Posted By : <%= Server.HtmlEncode(viewData.PostedBy) %>
    </div>
    <% if ((viewData.Stories != null) && (viewData.Stories.Length > 0)) %>
    <%{%>
        <uc:StoryListView runat="server"></uc:StoryListView>
    <%}%>
    <%else%>
    <%{%>
        <span class="pageMessage">No story submitted by this user.</span>
    <%}%>
</asp:Content>