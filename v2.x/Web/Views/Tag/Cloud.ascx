<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Cloud.ascx.cs" Inherits="Kigg.Web.TagCloud" %>
<%
double max = ViewData.Model.Max(t => t.StoryCount);
string[] cssClasses = new[] { "tag1", "tag2", "tag3", "tag4", "tag5", "tag6", "tag7" };

IOrderedEnumerable<ITag> tags = ViewData.Model.OrderBy(t => t.Name);

foreach (ITag tag in tags)
{
    int index = Convert.ToInt32(Math.Floor((tag.StoryCount / max) * cssClasses.Length));

    if (index == cssClasses.Length) index -= 1; //The last tag might exceed the css classes length.

    string cssClass = cssClasses[index];
%>
    <%= Html.ActionLink(tag.Name, "Tags", "Story", new { name = tag.UniqueName }, new { rel = "tag directory", @class = cssClass })%>
<%
}
%>