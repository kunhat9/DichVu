using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CORE.Helpers
{
    public static class XmlHelper
    {
        public static List<string> GetValueFromXml(string xml, string keyStart, string keyEnd)
        {
            List<string> result = new List<string>();

            string pattern = "<" + keyStart + ">(.*?)</" + keyEnd + ">";
            try
            {
                MatchCollection matches = Regex.Matches(xml, pattern);
                foreach (Match match in matches)
                {
                    if (keyStart.Equals(keyEnd))
                    {
                        result.Add(match.Groups[1].Value);
                    }
                    else
                    {
                        result.Add(xml.Substring(match.Index, match.Length));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("::GetValueFromXml::Error occured.\n" + ex.Message, ex);
            }
            return result;
        }
    }
}
