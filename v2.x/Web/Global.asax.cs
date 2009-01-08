namespace Kigg.Web
{
    using System;
    using System.Net;
    using System.Web;

    using Infrastructure;

    public class GlobalApplication : HttpApplication
    {
        public static void OnStart()
        {
            Bootstrapper.Run();
            Log.Info("Application Started");
        }

        public static void OnError(HttpServerUtilityBase server)
        {
            Exception e = server.GetLastError().GetBaseException();
            HttpException httpException = e as HttpException;

            // Ignore 404 Error
            if ((httpException != null) && (((HttpStatusCode) httpException.GetHttpCode()) == HttpStatusCode.NotFound))
            {
                return;
            }

            Log.Exception(e);
        }

        public static void OnEnd()
        {
            Log.Warning("Application Ended");
            IoC.Reset();
        }

        protected void Application_Start()
        {
            OnStart();
        }

        protected void Application_Error()
        {
            OnError(new HttpServerUtilityWrapper(Server));
        }

        protected void Application_End()
        {
            OnEnd();
        }
    }
}