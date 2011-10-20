<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<Category>>" %>
<span class="categoryRadioList">
<%foreach (Category category in Model.OrderBy(c => c.Name)) %>
<%{%>
    <label><input type="radio" name="category" value="<%= Html.AttributeEncode(category.UniqueName) %>"/> <%= Html.Encode(category.Name)%></label>
<%}%>
    <br class="clearLeft"/>
</span>