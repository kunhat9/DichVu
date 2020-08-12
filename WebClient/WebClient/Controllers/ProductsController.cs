using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebClient.Controllers
{
    public class ProductsController : BaseController
    {
        public ActionResult Index()
        {
            List<TB_PRODUCTS> list = Products_Service.GetAll();

            List<TB_FILES> file = Files_Service.GetAll().Where(x => x.FileType == "PRODUCT").ToList();
            ViewBag.Files = file;

            return View(list);
        }

        public ActionResult Detail(string Id = "")
        {
            TB_PRODUCTS d = Products_Service.GetById(Id);

            List<TB_FILES> file = Files_Service.GetByRefecense(Id).Where(x => x.FileType == "PRODUCT").ToList();
            ViewBag.Files = file;

            return View(d);
        }

        public ActionResult Search(string group = "", string key = "")
        {
            List<TB_CATEGORIES> catList = Categories_Service.GetAll();
            ViewBag.CatList = catList;
            ViewBag.Group = group;

            List<TB_PRODUCTS> productList = Products_Service.GetAll().Where(x => x.ProductName.IndexOf(key) > -1).ToList();
            ViewBag.ProductList = productList;

            List<TB_FILES> file = Files_Service.GetAll().Where(x => x.FileType == "PRODUCT").ToList();
            if (file == null) file = new List<TB_FILES>();
            ViewBag.Files = file;

            return View();
        }
    }
}