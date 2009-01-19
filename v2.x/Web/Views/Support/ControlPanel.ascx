<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlPanel.ascx.cs" Inherits="Kigg.Web.ControlPanel" %>
<div class="box">
    <h3>Control Panel</h3>
    <div>
        <% if (string.IsNullOrEmpty(ViewData.Model.ErrorMessage)) %>
        <% {%>
                <ul>
                    <li><%= Html.ActionLink("New ({0})".FormatWith(ViewData.Model.NewCount), "New", "Story") %></li>
                    <li><%= Html.ActionLink("Unapproved ({0})".FormatWith(ViewData.Model.UnapprovedCount), "Unapproved", "Story")%></li>
                    <li><%= Html.ActionLink("Publishable ({0})".FormatWith(ViewData.Model.PublishableCount), "Publishable", "Story")%></li>
                    <% if (ViewData.Model.IsAdministrator) %>
                    <% {%>
                        <li><a id="lnkPublish" href="javascript:void(0)">Publish</a></li>
                    <% }%>
                </ul>
        <% }%>
        <% else%>
        <% {%>
                <span style="font-weight:bold;color:#f00"><%= ViewData.Model.ErrorMessage %></span>
        <% }%>
    </div>
</div>
<div class="divider"></div>