using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class CartController : BaseController
    {
        public ActionResult Index()
        {
            List<CartModel> list = new List<CartModel>();
            if (Session[AppSession.AppSessionKeys.USER_CART] != null)
            {
                list = (List<CartModel>)Session[AppSession.AppSessionKeys.USER_CART];
            }

            List<TB_FILES> file = Files_Service.GetAll().Where(x => x.FileType == "PRODUCT").ToList();
            ViewBag.Files = file;

            return View(list);
        }

        public ActionResult Pay()
        {
            List<CartModel> list = new List<CartModel>();
            if (Session[AppSession.AppSessionKeys.USER_CART] != null)
            {
                list = (List<CartModel>)Session[AppSession.AppSessionKeys.USER_CART];
            }

            List<TB_FILES> file = Files_Service.GetAll().Where(x => x.FileType == "PRODUCT").ToList();
            ViewBag.Files = file;

            return View(list);
        }
    }
}