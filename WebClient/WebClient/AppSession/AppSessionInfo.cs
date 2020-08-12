using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClient.Cookie;

namespace WebClient.AppSession
{
    public class AppSessionInfo
    {
        private const string IS_AUTHRORIZED = "Is_Authorized";

        #region Private

        private static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        private static bool ExistValue(string key)
        {
            return (Context.Session[key] != null);
        }

        private static object GetValue(string key)
        {
            if (Context.Session[key] != null)
            {
                return Context.Session[key];
            }
            else
            {
                return null;
            }
        }

        private static void SetValue(string key, object value)
        {
            Context.Session[key] = value;
        }

        #endregion

        public static bool IsAuthorized
        {
            get
            {
                if (ExistValue(AppSessionInfo.IS_AUTHRORIZED))
                {
                    return (bool)GetValue(AppSessionInfo.IS_AUTHRORIZED);
                }
                else
                {
                    if (!string.IsNullOrEmpty(AppCookieInfo.UserID) && !string.IsNullOrEmpty(AppCookieInfo.HashedPassword))
                    {
                        var cookieUser = "User_ID";

                        if (cookieUser != null)
                        {
                            IsAuthorized = true;
                            AppSessionInfo.CurrentUser = cookieUser;

                            return true;
                        }
                        else
                        {
                            AppCookieInfo.RemoveAllCookies();

                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            private set
            {
                SetValue(AppSessionInfo.IS_AUTHRORIZED, value);
            }
        }
        
        public static object CurrentUser
        {
            get
            {
                if (!ExistValue(AppSessionKeys.USER_INFO))
                {
                    return null;
                }
                else
                {
                    return (object)GetValue(AppSessionKeys.USER_INFO);
                }
            }
            set
            {
                SetValue(AppSessionKeys.USER_INFO, value);
            }
        }
    }
}