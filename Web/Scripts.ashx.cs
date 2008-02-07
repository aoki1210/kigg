namespace Kigg
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Reflection;
    using System.Web;

    public class ScriptHandler : IHttpHandler
    {
        private static readonly string _versionNo = string.Empty;
        private static readonly bool _compress = true;
        private static readonly int _cacheDurationInDays = 0;

        private static readonly List<string> _files = new List<string>();

        public static string VersionNo
        {
            get
            {
                return _versionNo;
            }
        }

        bool IHttpHandler.IsReusable
        {
            get
            {
                return true;
            }
        }

        static ScriptHandler()
        {
            Hashtable settings = ConfigurationManager.GetSection("scriptSettings") as Hashtable;

            if (settings != null)
            {
                _versionNo = settings["versionNo"].ToString();
                _compress = Convert.ToBoolean(settings["compress"]);
                _cacheDurationInDays = Convert.ToInt32(settings["cacheDurationInDays"]);

                string fileList = settings["files"].ToString();

                if (!string.IsNullOrEmpty(fileList))
                {
                    string[] files = fileList.Split(new char[] {';', ','});

                    if (files.Length > 0)
                    {
                        HttpContext context = HttpContext.Current;

                        foreach (string file in files)
                        {
                            _files.Add(context.Server.MapPath(file));
                        }
                    }
                }
            }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;

            if (_files.Count == 0)
            {
                response.StatusCode = 500;
                response.StatusDescription = "Unable to find any script file.";
                response.End();
                return;
            }

            response.ContentType = "application/x-javascript";
            Stream output = response.OutputStream;

            //Compress
            if (_compress)
            {
                string acceptEncoding = context.Request.Headers["Accept-Encoding"];

                if (!string.IsNullOrEmpty(acceptEncoding))
                {
                    acceptEncoding = acceptEncoding.ToLowerInvariant();

                    if (acceptEncoding.Contains("gzip"))
                    {
                        response.AddHeader("Content-encoding", "gzip");
                        output = new GZipStream(output, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.Contains("deflate"))
                    {
                        response.AddHeader("Content-encoding", "deflate");
                        output = new DeflateStream(output, CompressionMode.Compress);
                    }
                }
            }

            //Combine
            using (StreamWriter sw = new StreamWriter(output))
            {
                //Write each files in the response
                foreach(string file in _files)
                {
                    string content;

                    using (Stream s = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            content = sr.ReadToEnd();
                        }
                    }

                    sw.WriteLine(content);
                }

                sw.Write("if(typeof(Sys)!='undefined'){Sys.Application.notifyScriptLoaded();}");
            }

            //Cache
            if (_cacheDurationInDays > 0)
            {
                TimeSpan duration = TimeSpan.FromDays(_cacheDurationInDays);

                HttpCachePolicy cache = response.Cache;

                cache.SetCacheability(HttpCacheability.Public);
                cache.SetExpires(DateTime.Now.Add(duration));
                cache.SetMaxAge(duration);
                cache.AppendCacheExtension("must-revalidate, proxy-revalidate");

                FieldInfo maxAgeField = cache.GetType().GetField("_maxAge", BindingFlags.Instance | BindingFlags.NonPublic);
                maxAgeField.SetValue(cache, duration);
            }
        }
    }
}