namespace Kigg
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;
    using System.Web;

    public class ScriptHandler : IHttpHandler
    {
        private static readonly string _versionNo;
        private static readonly bool _compress;
        private static readonly int _cacheDurationInDays;

        private static readonly List<string> _files = new List<string>();

        public static string VersionNo
        {
            get
            {
                return _versionNo;
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        static ScriptHandler()
        {
            var settings = ConfigurationManager.GetSection("scriptSettings") as Hashtable;

            if (settings != null)
            {
                _versionNo = settings["versionNo"].ToString();
                _compress = Convert.ToBoolean(settings["compress"], CultureInfo.InvariantCulture);
                _cacheDurationInDays = Convert.ToInt32(settings["cacheDurationInDays"], CultureInfo.InvariantCulture);

                var fileList = settings["files"].ToString();

                if (!string.IsNullOrEmpty(fileList))
                {
                    var files = fileList.Split(new[] {';', ','});

                    if (files.Length > 0)
                    {
                        var context = HttpContext.Current;

                        if (context != null)
                        {
                            foreach (var file in files)
                            {
                                _files.Add(context.Server.MapPath(file));
                            }
                        }
                    }
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            var response = context.Response;

            if (_files.Count == 0)
            {
                response.StatusCode = 500;
                response.StatusDescription = "Unable to find any script file.";
                response.End();
                return;
            }

            response.ContentType = "application/x-javascript";
            var output = response.OutputStream;

            //Compress
            if (_compress)
            {
                var acceptEncoding = context.Request.Headers["Accept-Encoding"];

                if (!string.IsNullOrEmpty(acceptEncoding))
                {
                    acceptEncoding = acceptEncoding.ToUpperInvariant();

                    if (acceptEncoding.Contains("GZIP"))
                    {
                        response.AddHeader("Content-encoding", "gzip");
                        output = new GZipStream(output, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.Contains("DEFLATE"))
                    {
                        response.AddHeader("Content-encoding", "deflate");
                        output = new DeflateStream(output, CompressionMode.Compress);
                    }
                }
            }

            //Combine
            using (var sw = new StreamWriter(output))
            {
                //Write each files in the response
                foreach(var file in _files)
                {
                    var content = File.ReadAllText(file);
                    sw.WriteLine(content);
                }

                sw.Write("if(typeof(Sys)!='undefined'){Sys.Application.notifyScriptLoaded();}");
            }

            //Cache
            if (_cacheDurationInDays > 0)
            {
                var duration = TimeSpan.FromDays(_cacheDurationInDays);

                var cache = response.Cache;

                cache.SetCacheability(HttpCacheability.Public);
                cache.SetExpires(DateTime.Now.Add(duration));
                cache.SetMaxAge(duration);
                cache.AppendCacheExtension("must-revalidate, proxy-revalidate");

                var maxAgeField = cache.GetType().GetField("_maxAge", BindingFlags.Instance | BindingFlags.NonPublic);
                maxAgeField.SetValue(cache, duration);
            }
        }
    }
}