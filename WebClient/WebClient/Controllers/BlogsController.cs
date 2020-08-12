using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebClient.Controllers
{
    public class BlogsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _DanhSach(string keyText = "", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_BLOGS> list = new List<TB_BLOGS>();

            int count = 0;
            try
            {
                list = Blogs_Service.GetAll();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Blogs/_List :", ex.Message, ex.ToString());
            }

            ViewBag.maxNumber = Math.Ceiling((double)count / pageSize);
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;

            return PartialView(list);
        }

        public PartialViewResult _ChiTiet(int blogId = 0)
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_BLOGS b = Blogs_Service.GetById(blogId);
            ViewBag.Blog = b;

            return PartialView(height);
        }
    }
}