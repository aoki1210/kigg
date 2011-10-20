<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<Tag>>" %>
<%
double max = Model.Max(t => t.StoryCount);
var cssClasses = new[] { "tag1", "tag2", "tag3", "tag4", "tag5", "tag6", "tag7" };

IOrderedEnumerable<Tag> tags = Model.OrderBy(t => t.Name);

foreach (Tag tag in tags)
{
    int index;

    try
    {
        index = Convert.ToInt32(Math.Floor((tag.StoryCount / max) * cssClasses.Length));
    }
    catch(OverflowException)
    {
        index = 0;
    }

    if (index == cssClasses.Length) index -= 1; //The last tag might exceed the css classes length.

    string cssClass = cssClasses[index];
%>
    <strong>
        <%= Html.ActionLink(tag.Name, "Tags", "Story", new { name = tag.UniqueName }, new { rel = "tag directory", @class = cssClass })%>
    </strong>
<%
}
%>