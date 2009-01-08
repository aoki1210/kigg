<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RadioButtonList.ascx.cs" Inherits="Kigg.Web.CategoryRadioList" %>
<span class="categoryRadioList">
<%foreach (ICategory category in ViewData.Model.OrderBy(c => c.Name)) %>
<%{%>
    <label><input type="radio" name="category" value="<%= Html.AttributeEncode(category.UniqueName) %>"/> <%= Html.Encode(category.Name)%></label>
<%}%>
    <br class="clearLeft"/>
</span>