using System.IO.Compression;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebAdmin.Configuration
{
    public class AjaxChildActionOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest() || controllerContext.IsChildAction;
        }
    }

    public class AjaxActionOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }
    }

    //public class CompressFilterAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        base.OnActionExecuting(filterContext);

    //        HttpRequestBase request = filterContext.HttpContext.Request;

    //        string acceptEncoding = request.Headers["Accept-Encoding"];

    //        if (string.IsNullOrEmpty(acceptEncoding)) return;

    //        acceptEncoding = acceptEncoding.ToUpperInvariant();

    //        HttpResponseBase response = filterContext.HttpContext.Response;

    //        if (acceptEncoding.Contains("GZIP"))
    //        {
    //            response.AppendHeader("Content-encoding", "gzip");
    //            response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
    //        }
    //        else if (acceptEncoding.Contains("DEFLATE"))
    //        {
    //            response.AppendHeader("Content-encoding", "deflate");
    //            response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
    //        }
    //    }
    //}
}