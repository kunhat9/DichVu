using System;
using System.Web;

namespace WebAdmin.Helpers
{
    public class CookieHelper
    {
        /// <summary>
        /// Set cookie value specified by the key.
        /// </summary>
        public static void SetCookie(string key, string value, int dayExpires)
        {
            HttpContext.Current.Response.Cookies[key].Value = value;
            HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(dayExpires);
        }

        public static void SetCookie(string key, string value)
        {
            SetCookie(key, value, 360);
        }

        /// <summary>
        /// Get cookie value specified by the key.
        /// </summary>
        public static string GetCookie(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                return HttpContext.Current.Request.Cookies[key].Value;
            }
            else
                return null;
        }

        /// <summary>
        /// Remove cookie specified by the key.
        /// </summary>
        public static void RemoveCookie(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
            }
        }

        public static void RemoveAllCookies()
        {
            foreach (string key in HttpContext.Current.Request.Cookies.AllKeys)
            {
                HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}