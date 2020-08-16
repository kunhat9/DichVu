using CORE.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebClient.Helpers;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class AjaxController : BaseController
    {
        public JsonResult FileUpload(HttpPostedFileBase file)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                string folderPath = "";// Configuration.AppConfigInfo.;
                string filePath = "";
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var extention = Path.GetExtension(file.FileName);
                    var filenamewithoutextension = Path.GetFileNameWithoutExtension(file.FileName);
                    if (!folderPath.EndsWith("/"))
                        filePath += "/" + DateTime.Now.ToString("yyyy_MM_dd") + "/";
                    else
                        filePath += DateTime.Now.ToString("yyyy_MM_dd") + "/";
                    IOHelper.CreateFolder(folderPath + filePath);
                    file.SaveAs(folderPath + filePath + file.FileName);

                    Result.Result = filePath + file.FileName;
                }

                Result.Code = 0;
            }
            catch
            {
                Result.Code = 2000;
                Result.Result = "Ajax Error From Server";
            }

            return Json(new JsonResult() { Data = Result });
        }

        #region Upload Photo

        public byte[] GetUploadedPhoto(FileUpload File)
        {
            using (Stream PhotoStream = File.PostedFile.InputStream)
            {
                long photoStreamLength = PhotoStream.Length;
                byte[] photoBytes = new byte[photoStreamLength + 1];
                PhotoStream.Read(photoBytes, 0, (int)photoStreamLength);
                return photoBytes;
            }
        }

        #endregion

        public JsonResult Register(int reg_service, string reg_datebegin, string reg_number, string reg_name, string reg_email, string reg_phone, string reg_note, string menuId, List<MenuModel> listMenu, string voucher, string reg_table)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                string menu = "";
                string reg_user = "";
                foreach (var s in listMenu)
                {
                    if (s != null)
                    {
                        menu += s.MdetailId + ",";
                    }

                }
                string result = Registers_Service.RegisterService(reg_service, reg_datebegin, reg_number, reg_name, reg_email, reg_phone, reg_note, reg_user, menuId, menu, voucher, reg_table);
                if (result.Equals("00"))
                {
                    Result.Code = 000;
                    Result.Result = "Thành công";
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult AddToCart(string productCode, int number)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                List<CartModel> list = new List<CartModel>();
                if (Session[AppSession.AppSessionKeys.USER_CART] != null)
                {
                    list = (List<CartModel>)Session[AppSession.AppSessionKeys.USER_CART];
                }

                if (list.Exists(x => x.ProductCode == productCode))
                {
                    foreach (CartModel item in list)
                    {
                        if (item.ProductCode == productCode)
                        {
                            item.Quantity += number;
                        }
                    }
                }
                else
                {
                    TB_PRODUCTS p = Products_Service.GetById(productCode);
                    list.Add(new CartModel
                    {
                        ProductCode = productCode,
                        ProductName = p.ProductName,
                        Quantity = number,
                        Price = p.ProductPrice
                    });
                }

                Session[AppSession.AppSessionKeys.USER_CART] = list;

                Result.Code = 000;
                Result.Result = list.Count;
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult RemoveFromCart(string productCode)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                List<CartModel> list = new List<CartModel>();
                if (Session[AppSession.AppSessionKeys.USER_CART] != null)
                {
                    list = (List<CartModel>)Session[AppSession.AppSessionKeys.USER_CART];
                }

                if (list.Exists(x => x.ProductCode == productCode))
                {
                    list.RemoveAll(x => x.ProductCode == productCode);
                }

                Session[AppSession.AppSessionKeys.USER_CART] = list;

                Result.Code = 000;
                Result.Result = list.Count;
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult GetPrice(int serviceId, string code = "", int menuId = 0, int num = 0)
        {
            AjaxResultModel Result = new AjaxResultModel();
            Result.Result = 0;

            try
            {
                TB_SERVICES s = Services_Service.GetById(serviceId);
                decimal p = s.ServicePrice;

                if (!string.IsNullOrEmpty(code))
                {
                    TB_VOUCHERS v = Voucher_Service.GetByCode(code);
                    if (v != null && v.VoucherDateExpired.Date >= DateTime.Now.Date && v.VoucherState == "A")
                    {
                        if (v.VoucherType == "M")//Giảm tiền
                        {
                            p = p - v.VoucherNum;
                        }
                        else if (v.VoucherType == "P")//Giảm phần trăm
                        {
                            p = p * (100 - v.VoucherNum) / 100;
                        }
                    }
                }

                if (num > 0 && menuId > 0)
                {
                    TB_MENUS m = Menu_Service.GetById(menuId);
                    if (m != null)
                    {
                        p += m.MenuPrice * num;
                    }
                }

                Result.Code = 000;
                Result.Result = string.Format("{0:N0}", p);
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = 0;
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }

        public JsonResult CartPay(string regName, string regPhone, string regMail, string regAddress, string regNote
            , string regNameOther, string regPhoneOther, string regMailOther, string regAddressOther
            , string voucher, string option)
        {
            AjaxResultModel Result = new AjaxResultModel();

            try
            {
                List<CartModel> list = new List<CartModel>();
                if (Session[AppSession.AppSessionKeys.USER_CART] != null)
                {
                    list = (List<CartModel>)Session[AppSession.AppSessionKeys.USER_CART];
                }

                string xmlOrder;
                xmlOrder = "<Order>"
                    + "<OrderName>" + regName + "</OrderName>"
                    + "<OrderPhone>" + regPhone + "</OrderPhone>"
                    + "<OrderMail>" + regMail + "</OrderMail>"
                    + "<OrderAddress>" + regAddress + "</OrderAddress>"
                    + "<OrgerNote>" + regNote + "</OrgerNote>"

                    + "<OrderNameOther>" + regNameOther + "</OrderNameOther>"
                    + "<OrderPhoneOther>" + regPhoneOther + "</OrderPhoneOther>"
                    + "<OrderMailOther>" + regMailOther + "</OrderMailOther>"
                    + "<OrderAddressOther>" + regAddressOther + "</OrderAddressOther>"

                    //+ "<OrderDateCreate>" + DateTime.Now + "</OrderDateCreate>"
                    //+ "<OrderTotal>" + 0 + "</OrderTotal>"
                    + "<OrderVoucher>" + voucher + "</OrderVoucher>"
                    + "<OrderOption>" + option + "</OrderOption>"
                    //+ "<OrderState>" + "A" + "</OrderState>"
                    + "</Order>";
                string xmlDetail;
                xmlDetail = "<row>";
                foreach (CartModel item in list)
                {
                    xmlDetail += "<detail>"
                        + "<ProductCode>" + item.ProductCode + "</ProductCode>"
                        + "<Quantity>" + item.Quantity + "</Quantity>"
                        + "</detail>";
                }
                xmlDetail += "</row>";
                string orderCode = "";
                if (Orders_Service.InsertOrder(xmlOrder, xmlDetail, out orderCode))
                {
                    Session[AppSession.AppSessionKeys.USER_CART] = null;
                    Result.Code = 000;
                    Result.Result = orderCode;
                }
                else
                {
                    Result.Code = 001;
                    Result.Result = "Không thành công";
                }
            }
            catch (Exception Ex)
            {
                Result.Code = 2000;
                Result.Result = "Có lỗi xảy ra. Vui lòng thử lại sau hoặc liên hệ với người quản trị.";
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "UpdatePassword :", Ex.Message, Ex.ToString());
            }

            return Json(Result);
        }
    }
}