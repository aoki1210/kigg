<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<ICategory>>" %>
<div class="category">
    <ul>
        <li>
            <a rel="home" href="<%= Url.Content("~") %>">All</a>
        </li>
        <%
        foreach (ICategory category in Model)
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