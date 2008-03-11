namespace Kigg
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    public class DataLoader : IHttpHandler
    {
        private Guid _currentUserID;
        private Category[] _categories;

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            _currentUserID = (Guid) Membership.GetUser().ProviderUserKey;

            context.Response.ContentType = "text/plain";

            if (_currentUserID == Guid.Empty)
            {
                context.Response.Write("You must be authenticated prior hitting this url");
            }
            else
            {
                using (CodeBenchmark.Start)
                {
                    //The Following loads 2000 Latest Story from Digg.com
                    IDataContext db = new KiggDataContext();

                    _categories = db.GetCategories();

                    int count = 0;
                    int max = (count + 20);
                    const string URL = "http://services.digg.com/stories/popular?appkey=http%3A%2F%2Fexample.com%2fapplication&type=xml&media=news&count=100&offset={0}";

                    while (count < max)
                    {
                        int offset = (count * 100);

                        string requestUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, URL, offset);
                        var request = (HttpWebRequest)WebRequest.Create(new Uri(requestUrl));

                        request.Method = "GET";
                        request.AllowAutoRedirect = true;
                        request.UserAgent = "Asp.net MVC Demo";
                        request.ContentType = "application/x-www-form-urlencoded";

                        string responseXml;

                        using (var response = (HttpWebResponse)request.GetResponse())
                        {
                            using (var sr = new StreamReader(response.GetResponseStream()))
                            {
                                responseXml = sr.ReadToEnd();
                            }
                        }

                        if (!string.IsNullOrEmpty(responseXml))
                        {
                            Export(db, responseXml);
                        }

                        count += 1;
                    }

                    context.Response.Write("Done");
                }
            }
        }

        private void Export(IDataContext db, string xml)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                if (xmlDoc.DocumentElement != null)
                {
                    XmlNodeList xnlStories = xmlDoc.DocumentElement.SelectNodes("story");

                    if ((xnlStories != null) && (xnlStories.Count > 0))
                    {
                        foreach (XmlNode xndStory in xnlStories)
                        {
                            try
                            {
                                string url = xndStory.Attributes["link"].Value;
                                string title = xndStory.SelectSingleNode("title").InnerText;
                                string description = xndStory.SelectSingleNode("description").InnerText;
                                string categoryName = xndStory.SelectSingleNode("container").Attributes["name"].Value;
                                int categoryId = _categories.First(c => c.Name == categoryName).ID;
                                string tag = xndStory.SelectSingleNode("topic").Attributes["name"].Value;

                                db.SubmitStory(url, title, categoryId, description, tag, _currentUserID);
                            }
                            catch (InvalidOperationException e)
                            {
                                //Skip Duplicate Story
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}