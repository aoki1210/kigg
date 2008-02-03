namespace Kigg
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    public class DataLoader : IHttpHandler
    {
        private Guid _currentUserID;
        private Category[] _categories;

        bool IHttpHandler.IsReusable
        {
            get
            {
                return false;
            }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            _currentUserID = (Guid) Membership.GetUser().ProviderUserKey;

            context.Response.ContentType = "text/plain";

            if (_currentUserID == Guid.Empty)
            {
                context.Response.Write("You must be authenticated prior hitting this url");
            }
            else
            {
                using (new CodeBenchmark())
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

                        HttpForm form = new HttpForm(string.Format(URL, offset));
                        form.Get();

                        string response = form.Response;

                        if (!string.IsNullOrEmpty(response))
                        {
                            Export(db, response);
                        }

                        count += 1;
                    }

                    context.Response.Write("Done");
                }
            }
        }

        private void Export(IDataContext db, string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNodeList xnlStories = xmlDoc.DocumentElement.SelectNodes("story");

            if (xnlStories.Count > 0)
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

        private class HttpForm
        {
            #region Instance Variables

            private string _url;
            private NameValueCollection _headers = new NameValueCollection();
            private StringBuilder _formFields = new StringBuilder();

            private string _response;

            #endregion

            #region Public Properties

            public string Url
            {
                [DebuggerStepThrough()]
                get
                {
                    return _url;
                }
                [DebuggerStepThrough()]
                set
                {
                    _url = value;
                }
            }

            public NameValueCollection Headers
            {
                get
                {
                    return _headers;
                }
            }

            public string Response
            {
                [DebuggerStepThrough()]
                get
                {
                    return _response;
                }
            }

            #endregion

            #region Constructors

            public HttpForm(string url)
            {
                _url = url;
            }

            public HttpForm()
            {
            }

            #endregion

            #region Public Methods

            public void AppendField(string name,
            string value)
            {
                if (_formFields.Length > 0)
                {
                    _formFields.Append("&");
                }

                _formFields.AppendFormat("{0}={1}", name, value);
            }

            public void AppendRawData(string data)
            {
                _formFields.AppendFormat("{0}", data);
            }

            public void Get()
            {
                //Basic Validation
                if (string.IsNullOrEmpty(_url))
                {
                    throw new InvalidOperationException("Url must be set before calling this method.");
                }

                string url = _url;

                if (_formFields.Length > 0)
                {
                    url += "?" + _formFields.ToString();
                }

                HttpWebRequest request = CreateRequest(url, "GET");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        _response = sr.ReadToEnd();
                    }
                }
            }

            public void Post()
            {
                //Basic Validation
                if (string.IsNullOrEmpty(_url))
                {
                    throw new InvalidOperationException("Url must be set before calling this method.");
                }

                HttpWebRequest request = CreateRequest(_url, "POST");

                byte[] content = Encoding.Default.GetBytes(_formFields.ToString());

                if ((content != null) && (content.Length > 0))
                {
                    request.ContentLength = content.Length;

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(content, 0, content.Length);
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        _response = sr.ReadToEnd();
                    }
                }
            }

            public void Reset()
            {
                _url = null;
                _headers.Clear();
                _formFields.Length = 0;
                _response = null;
            }

            private HttpWebRequest CreateRequest(string url, string method)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Headers.Add(Headers);
                request.Method = method;
                request.AllowAutoRedirect = true;
                request.UserAgent = "Asp.net MVC Demo";
                request.ContentType = "application/x-www-form-urlencoded";

                return request;
            }

            #endregion
        }
    }
}