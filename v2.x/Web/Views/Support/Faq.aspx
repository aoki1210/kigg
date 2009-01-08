<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteTemplate.Master" AutoEventWireup="true" CodeBehind="Faq.aspx.cs" Inherits="Kigg.Web.FaqView"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
    <script runat="server">

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.Header.Title = "{0} - FAQ".FormatWith(ViewData.Model.SiteTitle);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <%= Html.PageHeader("Frequently Asked Questions")%>
    <h2>Under Construction</h2>
</asp:Content>