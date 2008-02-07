namespace Kigg
{
    using System;
    using System.IO;
    using System.Text;
    using System.Reflection;
    using System.Collections;
    using System.Diagnostics;
    using System.Configuration;
    using System.Web;

    public class CodeBenchmark : IDisposable
    {
        private static readonly bool _enabled = false;
        private static readonly string _logFile = string.Empty;
        private static readonly bool _includeParameters = false;

        private readonly Stopwatch _watch;

        static CodeBenchmark()
        {
            Hashtable settings = ConfigurationManager.GetSection("codeBenchmark") as Hashtable;

            if (settings != null)
            {
                _enabled = Convert.ToBoolean(settings["enabled"]);
                _includeParameters = Convert.ToBoolean(settings["includeParameters"]);
                _logFile = AppDomain.CurrentDomain.BaseDirectory + (settings["logFile"] as string);
            }
        }

        [DebuggerStepThrough]
        public CodeBenchmark()
        {
            if (_enabled)
            {
                _watch = new Stopwatch();
                _watch.Start();
            }
        }

        [DebuggerStepThrough]
        public void Dispose()
        {
            if (_enabled)
            {
                _watch.Stop();

                DateTime end = DateTime.Now;
                DateTime start = end.AddMilliseconds(-_watch.ElapsedMilliseconds);

                HttpContext context = HttpContext.Current;

                if (context != null)
                {
                    string userName = "Anonymous";

                    string ipAddress = context.Request.UserHostAddress;
                    string url = context.Request.RawUrl;

                    if (context.User.Identity.IsAuthenticated)
                    {
                        userName = context.User.Identity.Name;
                    }

                    string methodInfo = GetCallingMethodDetails(_includeParameters);

                    using (FileStream fs = new FileStream(_logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"", userName, ipAddress, url, methodInfo, start, end, _watch.Elapsed));
                        }
                    }
                }
            }
        }

        private static string GetCallingMethodDetails(bool includeParameters)
        {
            StringBuilder output = new StringBuilder();
            //Skipping two Frame, First one is the current method and second one is the dispose method.
            StackTrace stackTrace = new StackTrace(2, false);

            StackFrame stackFrame = stackTrace.GetFrame(0);
            MethodBase method = stackFrame.GetMethod();

            output.Append(method.DeclaringType.FullName);
            output.Append(".");
            output.Append(method.Name);
            output.Append("(");

            if (includeParameters)
            {
                ParameterInfo[] paramInfos = method.GetParameters();

                if ((paramInfos != null) && (paramInfos.Length > 0))
                {
                    output.Append(paramInfos[0].ParameterType.ToString());
                    output.Append(" ");
                    output.Append(paramInfos[0].Name);

                    if (paramInfos.Length > 1)
                    {
                        for (int j = 1; j < paramInfos.Length; j++)
                        {
                            output.AppendFormat(", {0} {1}", paramInfos[j].ParameterType, paramInfos[j].Name);
                        }
                    }
                }
            }

            output.Append(")");

            return output.ToString();
        }
    }
}