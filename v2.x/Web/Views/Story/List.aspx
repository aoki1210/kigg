<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Kigg.Web.StoryListView"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
    <script runat="server">

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Page.Header.Title = ViewData.Model.Title;
        }

    </script>
    <link href="<%= ViewData.Model.RssUrl %>" title="<%= ViewData.Model.Title %> (rss)" type="application/rss+xml" rel="alternate"/>
    <link href="<%= ViewData.Model.AtomUrl %>" title="<%= ViewData.Model.Title %> (atom)" type="application/atom+xml" rel="alternate"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <span class="hide hfeed"><%= ViewData.Model.Title %></span>
    <% Html.RenderPartial("StoryList"); %>
</asp:Content>