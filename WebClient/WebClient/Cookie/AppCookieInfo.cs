using System;
using WebClient.Common;
using WebClient.Helpers;

namespace WebClient.Cookie
{
    public class AppCookieInfo
    {
        private static int GetInt(string key)
        {
            string ID = CookieHelper.GetCookie(key);

            if (string.IsNullOrEmpty(ID))
                return -1;
            try
            {
                return Convert.ToInt32(ID);
            }
            catch
            {
                return -1;
            }
        }

        public static string UserID
        {
            get
            {
                try
                {
                    string HashUserID = CookieHelper.GetCookie(AppCookieKeys.USERID);

                    if (string.IsNullOrEmpty(HashUserID))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        HashUserID = HashUserID.Substring(5, HashUserID.Length - 5);

                        return EncryptHelper.DecryptBase64(HashUserID);
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                string HashUserID = RandomGenerator.GetRandomString(5) + EncryptHelper.EncryptBase64(value.ToString());
                CookieHelper.SetCookie(AppCookieKeys.USERID, HashUserID);
            }
        }

        public static string HashedPassword
        {
            get
            {
                string fakePassword = CookieHelper.GetCookie(AppCookieKeys.HASHED_PASSWORD);
                if (string.IsNullOrEmpty(fakePassword))
                {
                    return string.Empty;
                }
                else
                {
                    return fakePassword.Substring(5, fakePassword.Length - 8);
                }
            }
            set
            {
                string fakePassword = RandomGenerator.GetRandomString(5) + value + RandomGenerator.GetRandomString(3);
                CookieHelper.SetCookie(AppCookieKeys.HASHED_PASSWORD, fakePassword);
            }
        }

        public static void RemoveAllCookies()
        {
            CookieHelper.RemoveCookie(AppCookieKeys.HASHED_PASSWORD);
            CookieHelper.RemoveCookie(AppCookieKeys.USERID);
        }
    }
}