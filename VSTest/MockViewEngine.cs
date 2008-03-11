namespace Kigg.VSTest
{
    using System.Web.Mvc;

    public class MockViewEngine : IViewEngine
    {
        public ViewContext ViewContext
        {
            get;
            private set;
        }

        public void RenderView(ViewContext viewContext)
        {
            ViewContext = viewContext;
        }
    }
}