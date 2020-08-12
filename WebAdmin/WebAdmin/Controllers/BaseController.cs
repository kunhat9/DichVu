using CORE.Services;
using CORE.Views;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using WebAdmin.Configuration;

namespace WebAdmin.Controllers
{
    [AuthorizeBusiness]
    public class BaseController : Controller
    {
        protected string StartUpPath
        {
            get
            {
                try
                {
                    return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                }
                catch (Exception)
                {
                    return "/Logs";
                }
            }
        }

        protected string IpAddress
        {
            get
            {
                string visitorIPAddress = "";
                try
                {
                    bool GetLan = false;
                    visitorIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (string.IsNullOrEmpty(visitorIPAddress))
                        visitorIPAddress = Request.ServerVariables["REMOTE_ADDR"];

                    if (string.IsNullOrEmpty(visitorIPAddress))
                        visitorIPAddress = Request.UserHostAddress;

                    if (string.IsNullOrEmpty(visitorIPAddress) || visitorIPAddress.Trim() == "::1")
                    {
                        GetLan = true;
                        visitorIPAddress = string.Empty;
                    }

                    if (GetLan)
                    {
                        if (string.IsNullOrEmpty(visitorIPAddress))
                        {
                            //This is for Local(LAN) Connected ID Address
                            string stringHostName = Dns.GetHostName();
                            //Get Ip Host Entry
                            IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                            //Get Ip Address From The Ip Host Entry Address List
                            IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                            try
                            {
                                visitorIPAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
                            }
                            catch
                            {
                                try
                                {
                                    visitorIPAddress = arrIpAddress[0].ToString();
                                }
                                catch
                                {
                                    try
                                    {
                                        arrIpAddress = Dns.GetHostAddresses(stringHostName);
                                        visitorIPAddress = arrIpAddress[0].ToString();
                                    }
                                    catch
                                    {
                                        visitorIPAddress = string.Empty;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CORE.Helpers.IOHelper.WriteLog(StartUpPath, "BaseController :", ex.Message, ex.ToString());
                }
                return visitorIPAddress;
            }
        }

        protected int UserId
        {
            get
            {
                try
                {
                    return ((VIEW_INFO_USER_LOGIN)Session[AppSession.AppSessionKeys.USER_INFO]).UserId;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        #region SERVICES

        protected TB_USERSFactory Users_Service = new TB_USERSFactory();
        protected TB_BLOGSFactory Blogs_Service = new TB_BLOGSFactory();
        protected TB_SERVICESFactory Services_Service = new TB_SERVICESFactory();
        protected TB_SERVICE_DETAILSFactory Service_Details_Service = new TB_SERVICE_DETAILSFactory();
        protected TB_TYPESFactory Types_Service = new TB_TYPESFactory();
        protected TB_SUPPLIERSFactory Suppliers_Service = new TB_SUPPLIERSFactory();
        protected TB_PRODUCTSFactory Products_Service = new TB_PRODUCTSFactory();
        protected TB_REGISTERSFactory Registers_Service = new TB_REGISTERSFactory();
        protected TB_FILESFactory Files_Service = new TB_FILESFactory();
        protected TB_CATEGORIESFactory Categories_Service = new TB_CATEGORIESFactory();
        protected TB_SLIDERSFactory Sliders_Service = new TB_SLIDERSFactory();
        protected TB_ORDERSFactory Orders_Service = new TB_ORDERSFactory();
        protected TB_VOUCHERSFactory Voucher_Service = new TB_VOUCHERSFactory();
        protected TB_MENUSFactory Menus_Service = new TB_MENUSFactory();

        #endregion

        protected void GetLoginInfo(string username, string password)
        {
            try
            {

            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ADM"), GetIP(), ex.ToString());
            }
        }

        protected string GetMD5Hash(string rawString)
        {
            UnicodeEncoding encode = new UnicodeEncoding();
            //Chuyển kiểu chuổi thành kiểu byte
            Byte[] passwordBytes = encode.GetBytes(rawString);
            //mã hóa chuỗi đã chuyển
            Byte[] hash;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            hash = md5.ComputeHash(passwordBytes);
            //tạo đối tượng StringBuilder (làm việc với kiểu dữ liệu lớn)
            StringBuilder sb = new StringBuilder();
            foreach (byte outputByte in hash)
            {
                sb.Append(outputByte.ToString("x2").ToUpper());
                //nếu bạn muốn các chữ cái in thường thay vì in hoa thì bạn thay chữ "X" in hoa 
                //trong "X2" thành "x"
            }
            return sb.ToString();
        }

        protected string GetIP()
        {
            try
            {
                return Request.UserHostAddress;
            }
            catch (Exception) { return ""; }
        }
    }
}