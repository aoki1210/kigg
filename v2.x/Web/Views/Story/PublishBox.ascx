<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublishBox.ascx.cs" Inherits="Kigg.Web.PublishBox" %>
<% int count = (int) ViewData["count"];%>
<% if (count > 0)
   {
%>
    <% using (Html.BeginForm("Publish", "Story", FormMethod.Post, new { id = "frmPublish" }))%>
    <% { %>
        <div class="box">
            <h3>Publish</h3>
            <div style="text-align:center">
                <input type="submit" class="largeButton" value="Ready (<%= count %>)"/>
            </div>
        </div>
    <% } %>
    <div class="divider"></div>
<%
  }
%>