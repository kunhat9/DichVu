using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebClient.Controllers
{
    public class NewsController : BaseController
    {
        public ActionResult Index(string type = "")
        {
            List<TB_BLOGS> l = Blogs_Service.GetAll().Where(x => string.IsNullOrEmpty(type) || x.BlogType == type).ToList();
            ViewBag.Type = type;

            try
            {
                ViewBag.Images = Files_Service.GetAll().Where(x => x.FileType == "BLOG").ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Blogs/_List :", ex.Message, ex.ToString());
            }

            return View(l);
        }

        public ActionResult Detail(int Id = 0)
        {
            TB_BLOGS b = Blogs_Service.GetById(Id);
            return View(b);
        }
    }
}