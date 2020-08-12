using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAdmin.Controllers
{
    public class ProductsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _DanhSach(string keyText = "", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_PRODUCTS> list = new List<TB_PRODUCTS>();

            int count = 0;
            try
            {
                keyText = keyText.Trim();
                list = Products_Service.GetAll()
                 .Where(x => string.IsNullOrEmpty(keyText) || x.ProductCode.IndexOf(keyText) >= 0 || x.ProductName.IndexOf(keyText) >= 0)
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

            try
            {
                ViewBag.SupList = Suppliers_Service.GetAll();
                ViewBag.CatList = Categories_Service.GetAll();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Services/_List :", ex.Message, ex.ToString());
            }

            return PartialView(list);
        }

        public PartialViewResult _ChiTiet(string productId = "")
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_PRODUCTS s = Products_Service.GetById(productId);
            ViewBag.Product = s;

            List<TB_SUPPLIERS> list = Suppliers_Service.GetAll()
                .Where(x => x.SupplierState == "A").ToList();
            ViewBag.SupplierList = list;

            List<TB_CATEGORIES> list2 = Categories_Service.GetAll()
                .Where(x => x.CategoryState == "A").ToList();
            ViewBag.CategoryList = list2;

            try
            {
                ViewBag.Images = Files_Service.GetByRefecense("" + s.ProductCode).Where(x => x.FileType == "PRODUCT").ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Blogs/_List :", ex.Message, ex.ToString());
            }

            return PartialView(height);
        }

        public ActionResult SupplierList()
        {
            return View();
        }

        public PartialViewResult _SupplierList(string keyText = "", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_SUPPLIERS> list = new List<TB_SUPPLIERS>();

            int count = 0;
            try
            {
                keyText = keyText.Trim();
                list = Suppliers_Service.GetAll()
                 .Where(x => string.IsNullOrEmpty(keyText) || x.SupplierCode.IndexOf(keyText) >= 0 || x.SupplierName.IndexOf(keyText) >= 0)
                 .ToList();
                count = list.Count;
                list = list
                 .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                 .ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Products/_SupplierList :", ex.Message, ex.ToString());
            }

            ViewBag.maxNumber = Math.Ceiling((double)count / pageSize);
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;

            return PartialView(list);
        }

        public PartialViewResult _SupplierListDetail(string supplierCode = "")
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_SUPPLIERS sup = Suppliers_Service.GetById(supplierCode);
            ViewBag.Supplier = sup;

            return PartialView(height);
        }

        public ActionResult CategoryList()
        {
            return View();
        }

        public PartialViewResult _CategoryList(string keyText = "", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_CATEGORIES> list = new List<TB_CATEGORIES>();

            int count = 0;
            try
            {
                keyText = keyText.Trim();
                list = Categories_Service.GetAll()
                 .Where(x => string.IsNullOrEmpty(keyText) || x.CategoryCode.IndexOf(keyText) >= 0 || x.CategoryName.IndexOf(keyText) >= 0)
                 .ToList();
                count = list.Count;
                list = list
                 .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                 .ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Products/_CategoryList :", ex.Message, ex.ToString());
            }

            ViewBag.maxNumber = Math.Ceiling((double)count / pageSize);
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;

            return PartialView(list);
        }

        public PartialViewResult _CategoryListDetail(string categoryCode = "")
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_CATEGORIES sup = Categories_Service.GetById(categoryCode);
            ViewBag.Category = sup;

            return PartialView(height);
        }
    }
}