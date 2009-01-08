<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Kigg.Web.AboutView"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
    <script runat="server">

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.Header.Title = "{0} - About Us".FormatWith(ViewData.Model.SiteTitle);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <%= Html.PageHeader("About Us")%>
    <h2>Under Construction</h2>
</asp:Content>