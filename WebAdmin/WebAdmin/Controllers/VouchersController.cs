using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAdmin.Controllers
{
    public class VouchersController : BaseController
    {
        public ActionResult Index()
        {
            List<TB_SERVICES> service = new List<TB_SERVICES>();
            try
            {
                service = Services_Service.GetAll();
            }catch(Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Sliders/_List :", ex.Message, ex.ToString());
            }
            ViewBag.ListService = service;
            return View();
        }
        public PartialViewResult _DanhSach(string keyText = "", string startDate ="", string endDate="" ,string serviceId = "", string status = "A", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_VOUCHERS> list = new List<TB_VOUCHERS>();
            List<TB_SERVICES> service = new List<TB_SERVICES>();
            int count = 0;
            try
            {
                service = Services_Service.GetAll();
                keyText = keyText.Trim();
                list = Voucher_Service.GetAll()
                 .Where(x => (string.IsNullOrEmpty(keyText) || x.VoucherCode.IndexOf(keyText) >= 0 || x.VoucherNote.IndexOf(keyText) >= 0 )
                 && x.VoucherState.Equals(status)
                 && (!string.IsNullOrEmpty(startDate) ? Int32.Parse(x.VoucherDateCreate.ToString("yyyyMMdd")) >= Int32.Parse(startDate) : true)
                && (!string.IsNullOrEmpty(startDate) ? Int32.Parse(x.VoucherDateExpired.ToString("yyyyMMdd")) <= Int32.Parse(endDate) : true)
                 && (string.IsNullOrEmpty(serviceId) ? true : x.VoucherServiceId.Equals(serviceId))
                 )
                 .ToList();
                count = list.Count;
                list = list
                 .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                 .ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Sliders/_List :", ex.Message, ex.ToString());
            }

            ViewBag.maxNumber = Math.Ceiling((double)count / pageSize);
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.ListService = service;

            return PartialView(list);

        }
        public PartialViewResult _ChiTiet(string voucherId = "")
        {
            List<TB_SERVICES> service = new List<TB_SERVICES>();
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);
            TB_VOUCHERS b = new TB_VOUCHERS();

            try
            {
                service = Services_Service.GetAll();
                b = Voucher_Service.GetById(voucherId);
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Blogs/_List :", ex.Message, ex.ToString());
            }
            ViewBag.ListService = service;
            ViewBag.Voucher = b;
            return PartialView(height);
        }
    }
}