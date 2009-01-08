<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="Kigg.Web.CategoryMenu" %>
<div class="category">
    <ul>
        <li>
            <a rel="home" href="<%= Url.Content("~") %>">All</a>
        </li>
        <%
        foreach (ICategory category in ViewData.Model)
        {
        %>
            <li>
                <%= Html.ActionLink(category.Name, "Category", "Story", new { name = category.UniqueName }, new { rel = "tag directory" })%>
            </li>
        <%
        }
        %>
    </ul>
</div>