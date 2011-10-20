<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<Category>>"%>
<div class="category">
    <ul>
        <li>
            <strong><a rel="home" href="<%= Url.Content("~") %>">All</a></strong>
        </li>
        <%
        foreach (Category category in Model)
        {
        %>
            <li>
                <strong>
                    <%= Html.ActionLink(category.Name, "Category", "Story", new { name = category.UniqueName }, new { rel = "tag directory" })%>
                </strong>
            </li>
        <%
        }
        %>
    </ul>
</div>