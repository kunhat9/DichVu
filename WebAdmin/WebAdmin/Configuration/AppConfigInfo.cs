using System.Configuration;

namespace WebAdmin.Configuration
{
    public class AppConfigInfo
    {
        #region Private

        private static int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        private static int GetInt(string key, int defaultValue)
        {
            int result = 0;
            if (int.TryParse(ConfigurationManager.AppSettings[key], out result))
                return result;
            else
                return defaultValue;
        }

        private static bool GetBool(string key)
        {
            return GetBool(key, false);
        }

        private static bool GetBool(string key, bool defaultValue)
        {
            bool result = false;
            if (bool.TryParse(ConfigurationManager.AppSettings[key], out result))
                return result;
            else
                return defaultValue;
        }

        #endregion
    }
}