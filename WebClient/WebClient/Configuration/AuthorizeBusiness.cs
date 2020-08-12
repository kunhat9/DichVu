using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClient.AppSession;

namespace WebClient.Configuration
{
    public class AuthorizeBusiness : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            List<string> notCheck = new List<string> {
                "/Home/Login",
                "/Home/Logout",
                "/Home/_HomeHeader",
                "/Home/_HomeFooter",
                "/Home/_HomeMenuLeft",
                "/Home/_Pagination",
                "/Home/NotPermission",
                "/Home/_NotPermission",
                "/Home/_HomeCategory"
            };
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionUrl = "/" + controllerName + "/" + actionName;

            if (notCheck.Any(x => string.Equals(x, actionUrl, System.StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }
            if (HttpContext.Current.Session[AppSessionKeys.USER_INFO] == null)
            {
                HttpContext.Current.Session[AppSessionKeys.USER_INFO] = new CORE.Views.VIEW_INFO_USER_LOGIN();
                //string url = HttpContext.Current.Request.CurrentExecutionFilePath;
                //if (!string.Equals(actionUrl, "/Home/Login", System.StringComparison.OrdinalIgnoreCase))
                //{
                //    string redirect = "/Login";
                //    if (!string.IsNullOrEmpty(url) && url != "/")
                //    {
                //        redirect += "?url=" + url;
                //    }
                //    filterContext.Result = new RedirectResult(redirect);
                //    return;
                //}
            }
        }
    }
}