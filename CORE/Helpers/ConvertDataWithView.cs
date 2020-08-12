using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Helpers
{
    public static class ConvertDataWithView
    {
        public static string Date_yyyyMMdd_ToView(this string date, bool now = false)
        {
            try
            {
                return string.IsNullOrEmpty(date) ? now ? DateTime.Now.ToString("dd/MM/yyyy") : "" : DateTime.ParseExact(date, "yyyyMMdd", null).ToString("dd/MM/yyyy");
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string Currency_ToView(this decimal currency, bool def = false)
        {
            try
            {
                return currency == 0 ? def ? "0" : "" : string.Format("{0:N0}", currency);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string Int_ToView(this int i)
        {
            try
            {
                return i == 0 ? "" : "" + i;
            }
            catch (Exception)
            {
                return "";
            }
        }
        
    }
}
