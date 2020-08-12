using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CORE.Helpers
{
    public static class StringHelper
    {
        public static int IndexOf(this string input, string[] values, int start = 0, bool ignoreCase = true)
        {
            int first = -1;

            foreach (string item in values)
            {
                int i = -1;
                if (ignoreCase)
                    i = input.IndexOf(item.ToUpper(), start, System.StringComparison.OrdinalIgnoreCase);
                else
                    i = input.IndexOf(item, start);
                if (i >= 0)
                {
                    if (first >= 0)
                    {
                        if (i < first)
                        {
                            first = i;
                        }
                    }
                    else
                    {
                        first = i;
                    }
                }
            }
            if (first == 0) first = -1;
            return first;
        }

        public static string ValueOf(this string input, string[] values, int start = 0, bool ignoreCase = true)
        {
            int first = -1;
            string value = "";

            foreach (string item in values)
            {
                int i = -1;
                if (ignoreCase)
                    i = input.IndexOf(item.ToUpper(), start, System.StringComparison.OrdinalIgnoreCase);
                else
                    i = input.IndexOf(item, start);
                if (i >= 0)
                {
                    if (first >= 0)
                    {
                        if (i < first)
                        {
                            value = input.Substring(i, item.Length - i);
                            first = i;
                        }
                        else
                        {
                            if (item.Length > value.Length)
                            {
                                value = input.Substring(i, item.Length - i);
                                first = i;
                            }
                        }
                    }
                    else
                    {
                        value = input.Substring(i, item.Length - i);
                        first = i;
                    }
                }
            }
            return value;
        }

        public static int IndexOfNumber(this string input, int start = 0)
        {
            return input.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, start);
        }

        public static string GetFirstNumber(this string input, int length = 2)
        {
            try
            {
                return Regex.Replace(input.Trim(), @"\D+.*", "");
            }
            catch (System.Exception)
            {
                return "";
            }
        }

        public static string GetLottery(this string input, int length = 2)
        {
            return "0000".Substring(0, length - (input).Length) + input;
        }

        public static string RemoveSpecial(this string input)
        {
            input = Regex.Replace(input, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ", "a");
            input = Regex.Replace(input, "À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ", "A");
            input = Regex.Replace(input, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ", "e");
            input = Regex.Replace(input, "È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ", "E");
            input = Regex.Replace(input, "ì|í|ị|ỉ|ĩ", "i");
            input = Regex.Replace(input, "Ì|Í|Ị|Ỉ|Ĩ", "I");
            input = Regex.Replace(input, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ", "o");
            input = Regex.Replace(input, "Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ", "O");
            input = Regex.Replace(input, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ", "u");
            input = Regex.Replace(input, "Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ", "U");
            input = Regex.Replace(input, "ỳ|ý|ỵ|ỷ|ỹ", "y");
            input = Regex.Replace(input, "Ỳ|Ý|Ỵ|Ỷ|Ỹ", "Y");
            input = Regex.Replace(input, "Đ", "D");
            input = Regex.Replace(input, "đ", "d");

            return input;
        }

        public static string RemoveSpecialHtml(this string input)
        {
            input = Regex.Replace(input, "&quot;", "\"");
            input = Regex.Replace(input, "&lt;", "<");
            input = Regex.Replace(input, "&gt;", ">");
            input = Regex.Replace(input, "&amp;&nbsp;", " ");
            input = Regex.Replace(input, "&nbsp;", " ");
            input = Regex.Replace(input, "&iexcl;", "i");
            input = Regex.Replace(input, "&macr;", "-");
            input = Regex.Replace(input, "&ordf", "a");
            input = Regex.Replace(input, "&plusmn;", "+|-");
            input = Regex.Replace(input, "&sup1;", "1");
            input = Regex.Replace(input, "&sup2;", "2");
            input = Regex.Replace(input, "&sup3;", "3");
            input = Regex.Replace(input, "&ordm", "0");
            input = Regex.Replace(input, "&raquo", "»");
            input = Regex.Replace(input, "&frac14", "1/4");
            input = Regex.Replace(input, "&frac12", "1/2");
            input = Regex.Replace(input, "&frac34", "3/4");
            input = Regex.Replace(input, "&times;", "x");
            input = Regex.Replace(input, "&divide;", "÷");
            input = Regex.Replace(input, "&Agrave;", "À");
            input = Regex.Replace(input, "&Aacute;", "Á");
            input = Regex.Replace(input, "&Acirc;", "Â");
            input = Regex.Replace(input, "&Atilde;", "Ã");
            input = Regex.Replace(input, "&Auml;", "Ä");
            input = Regex.Replace(input, "&Aring;", "Å");
            input = Regex.Replace(input, "&AElig;", "Æ");
            input = Regex.Replace(input, "&Ccedil;", "Ç");
            input = Regex.Replace(input, "&Egrave;", "È");
            input = Regex.Replace(input, "&Eacute;", "É");
            input = Regex.Replace(input, "&Ecirc;", "Ê");
            input = Regex.Replace(input, "&Euml;", "Ë");
            input = Regex.Replace(input, "&Igrave;", "Ì");
            input = Regex.Replace(input, "&Iacute;", "Í");
            input = Regex.Replace(input, "&Icirc;", "Î");
            input = Regex.Replace(input, "&Iuml;", "Ï");
            input = Regex.Replace(input, "&ETH;", "Ð");
            input = Regex.Replace(input, "&Ntilde;", "Ñ");
            input = Regex.Replace(input, "&Ograve;", "Ò");
            input = Regex.Replace(input, "&Oacute;", "Ó");
            input = Regex.Replace(input, "&Ocirc;", "Ô");
            input = Regex.Replace(input, "&Otilde;", "Õ");
            input = Regex.Replace(input, "&Ouml;", "Ö");
            input = Regex.Replace(input, "&Oslash;", "Ø");
            input = Regex.Replace(input, "&Ugrave;", "Ù");
            input = Regex.Replace(input, "&Uacute;", "Ú");
            input = Regex.Replace(input, "&Ucirc;", "Û");
            input = Regex.Replace(input, "&Uuml;", "Ü");
            input = Regex.Replace(input, "&Yacute;", "Ý");
            input = Regex.Replace(input, "&THORN;", "Þ");
            input = Regex.Replace(input, "&szlig;", "ß");
            input = Regex.Replace(input, "&agrave;", "à");
            input = Regex.Replace(input, "&aacute;", "á");
            input = Regex.Replace(input, "&acirc;", "â");
            input = Regex.Replace(input, "&atilde;", "ã");
            input = Regex.Replace(input, "&auml;", "ä");
            input = Regex.Replace(input, "&aring;", "å");
            input = Regex.Replace(input, "&aelig;", "æ");
            input = Regex.Replace(input, "&ccedil;", "ç");
            input = Regex.Replace(input, "&egrave;", "è");
            input = Regex.Replace(input, "&eacute;", "é");
            input = Regex.Replace(input, "&ecirc;", "ê");
            input = Regex.Replace(input, "&euml;", "ë");
            input = Regex.Replace(input, "&igrave;", "ì");
            input = Regex.Replace(input, "&iacute;", "í");
            input = Regex.Replace(input, "&icirc;", "î");
            input = Regex.Replace(input, "&iuml;", "ï");
            input = Regex.Replace(input, "&eth;", "ð");
            input = Regex.Replace(input, "&ntilde;", "ñ");
            input = Regex.Replace(input, "&ograve;", "ò");
            input = Regex.Replace(input, "&oacute;", "ó");
            input = Regex.Replace(input, "&ocirc;", "ô");
            input = Regex.Replace(input, "&otilde;", "õ");
            input = Regex.Replace(input, "&oslash;", "ø");
            input = Regex.Replace(input, "&ugrave;", "ù");
            input = Regex.Replace(input, "&uacute;", "ú");
            input = Regex.Replace(input, "&ucirc;", "û");
            input = Regex.Replace(input, "&uuml;", "ü");
            input = Regex.Replace(input, "&yacute;", "ý");
            input = Regex.Replace(input, "&thorn;", "þ");
            input = Regex.Replace(input, "&yuml;", "ÿ");
            input = Regex.Replace(input, "&amp;", "&");
            input = Regex.Replace(input, "&amp;ecirc;", "ê");
            input = Regex.Replace(input, "&amp;aacute;", "á");
            input = Regex.Replace(input, "&amp;oacute;", "ó");
            input = Regex.Replace(input, "&amp;atilde;", "ã");
            input = Regex.Replace(input, "&amp;agrave;", "à");
            input = Regex.Replace(input, "&amp;ocirc;", "ô");
            input = Regex.Replace(input, "&amp;igrave;", "ì");
            input = Regex.Replace(input, "&amp;Agrave;", "À");
            input = Regex.Replace(input, "&amp;Aacute;", "Á");
            input = Regex.Replace(input, "&amp;Acirc;", "Â");
            input = Regex.Replace(input, "&amp;Atilde;", "Ã");
            input = Regex.Replace(input, "&amp;Auml;", "Ä");
            input = Regex.Replace(input, "&amp;Aring;", "Å");
            input = Regex.Replace(input, "&amp;AElig;", "Æ");
            input = Regex.Replace(input, "&amp;Egrave;", "È");
            input = Regex.Replace(input, "&amp;Ccedil;", "Ç");
            input = Regex.Replace(input, "&amp;Eacute;", "É");
            input = Regex.Replace(input, "&amp;Ecirc;", "Ê");
            input = Regex.Replace(input, "&amp;Euml;", "Ë");
            input = Regex.Replace(input, "&amp;Igrave;", "Ì");
            input = Regex.Replace(input, "&amp;&Iacute;", "Í");
            input = Regex.Replace(input, "&amp;&Icirc;", "Î");
            input = Regex.Replace(input, "&amp;&Iuml;", "Ï");
            input = Regex.Replace(input, "&amp;&ETH;", "Ð");
            input = Regex.Replace(input, "&amp;&Ntilde;", "Ñ");
            input = Regex.Replace(input, "&amp;&Ograve;", "Ò");
            input = Regex.Replace(input, "&amp;&Oacute;", "Ó");
            input = Regex.Replace(input, "&amp;&Ocirc;", "Ô");
            input = Regex.Replace(input, "&amp;&Otilde;", "Õ");
            input = Regex.Replace(input, "&amp;&Ouml;", "Ö");
            input = Regex.Replace(input, "&amp;&Oslash;", "Ø");
            input = Regex.Replace(input, "&amp;&Ugrave;", "Ù");
            input = Regex.Replace(input, "&amp;&Uacute;", "Ú");
            input = Regex.Replace(input, "&amp;&Ucirc;", "Û");
            input = Regex.Replace(input, "&amp;&Uuml;", "Ü");
            input = Regex.Replace(input, "&amp;&Yacute;", "Ý");
            input = Regex.Replace(input, "&amp;&THORN;", "Þ");
            input = Regex.Replace(input, "&amp;&szlig;", "ß");
            input = Regex.Replace(input, "&amp;&acirc;", "â");
            input = Regex.Replace(input, "&amp;&auml;", "ä");
            input = Regex.Replace(input, "&amp;&aring;", "å");
            input = Regex.Replace(input, "&amp;&aelig;", "æ");
            input = Regex.Replace(input, "&amp;&ccedil;", "ç");
            input = Regex.Replace(input, "&amp;&egrave;", "è");
            input = Regex.Replace(input, "&amp;&eacute;", "é");
            input = Regex.Replace(input, "&amp;&euml;", "ë");
            input = Regex.Replace(input, "&amp;&igrave;", "ì");
            input = Regex.Replace(input, "&amp;&iacute;", "í");
            input = Regex.Replace(input, "&amp;&icirc;", "î");
            input = Regex.Replace(input, "&amp;&iuml;", "ï");
            input = Regex.Replace(input, "&amp;&eth;", "ð");
            input = Regex.Replace(input, "&amp;&ntilde;", "ñ");
            input = Regex.Replace(input, "&amp;&ograve;", "ò");
            input = Regex.Replace(input, "&amp;&otilde;", "õ");
            input = Regex.Replace(input, "&amp;&oslash;", "ø");
            input = Regex.Replace(input, "&amp;&ugrave;", "ù");
            input = Regex.Replace(input, "&amp;&uacute;", "ú");
            input = Regex.Replace(input, "&amp;&ucirc;", "û");
            input = Regex.Replace(input, "&amp;&uuml;", "ü");
            input = Regex.Replace(input, "&amp;&yacute;", "ý");
            input = Regex.Replace(input, "&amp;&thorn;", "þ");
            input = Regex.Replace(input, "&amp;&yuml;", "ÿ");

            return input;
        }

        public static T[] Add<T>(this T[] a, T[] b)
        {
            return new List<T>().Concat(a).Concat(b).ToArray();
        }

        public static bool ArrayEquals<T>(this T[] a, T[] b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (!b.Contains(a[i]))
                    return false;
            }

            return true;
        }
    }
}
