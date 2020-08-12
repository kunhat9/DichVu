using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAdmin.Controllers
{
    public class OrdersController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _DanhSach(string keyText = "", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_ORDERS> list = new List<TB_ORDERS>();

            int count = 0;
            try
            {
                keyText = keyText.Trim().ToLower();
                list = Orders_Service.GetAll()
                    .Where(x => string.IsNullOrEmpty(keyText)
                        || x.OrderCode.ToLower().IndexOf(keyText) >= 0)
                    .ToList();
                count = list.Count;
                list = list
                 .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                 .ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Services/_List :", ex.Message, ex.ToString());
            }

            ViewBag.maxNumber = Math.Ceiling((double)count / pageSize);
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;

            return PartialView(list);
        }

        public PartialViewResult _ChiTiet(string orderCode = "")
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_ORDERS s = Orders_Service.GetById(orderCode);
            ViewBag.Order = s;

            //List<TB_SUPPLIERS> list = Suppliers_Service.GetAll()
            //    .Where(x => x.SupplierState == "A").ToList();
            //ViewBag.SupplierList = list;

            return PartialView(height);
        }
    }
}