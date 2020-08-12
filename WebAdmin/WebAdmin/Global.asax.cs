using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebAdmin.App_Start;

namespace WebAdmin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Init_Container();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CustomViewEngine());
        }

        protected void Init_Container()
        {
            //var builder = new ContainerBuilder();
            //builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            //builder.Register(d => new _Entities()).InstancePerRequest();
            //var assembly = Assembly.GetAssembly(typeof(_Servicces));
            //builder.RegisterAssemblyTypes(assembly).Where(x => x.Namespace != null && x.Namespace.EndsWith("Services.Impl")).AsImplementedInterfaces();
            //var container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    // Implement HTTP compression
        //    HttpApplication app = (HttpApplication)sender;

        //    // Retrieve accepted encodings
        //    string encodings = app.Request.Headers.Get("Accept-Encoding");
        //    if (encodings != null)
        //    {
        //        // Check the browser accepts deflate or gzip (deflate takes preference)
        //        encodings = encodings.ToLower();
        //        if (encodings.Contains("deflate"))
        //        {
        //            app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
        //            app.Response.AppendHeader("Content-Encoding", "deflate");
        //        }
        //        else if (encodings.Contains("gzip"))
        //        {
        //            app.Response.Filter = new GZipStream(app.Response.Filter, CompressionMode.Compress);
        //            app.Response.AppendHeader("Content-Encoding", "gzip");
        //        }
        //    }
        //}
    }

    public class CustomViewEngine : RazorViewEngine
    {
        public CustomViewEngine()
        {
            //var viewLocations = new[] {
            //    "~/Views/{1}/{0}.aspx",
            //    "~/Views/{1}/{0}.ascx",
            //    "~/Views/Shared/{0}.aspx",
            //    "~/Views/Shared/{0}.ascx",
            //    "~/AnotherPath/Views/{0}.ascx"
            //    // etc
            //};

            //this.PartialViewLocationFormats = viewLocations;
            //this.ViewLocationFormats = viewLocations;
        }

        public void AddViewLocationFormat(string paths)
        {
            List<string> existingPaths = new List<string>(ViewLocationFormats);
            existingPaths.Add(paths);

            ViewLocationFormats = existingPaths.ToArray();
        }

        public void AddPartialViewLocationFormat(string paths)
        {
            List<string> existingPaths = new List<string>(PartialViewLocationFormats);
            existingPaths.Add(paths);

            PartialViewLocationFormats = existingPaths.ToArray();
        }
    }
}
