using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebAdmin.Controllers
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
                keyText = keyText.Trim();
                list = Blogs_Service.GetAll()
                 .Where(x => string.IsNullOrEmpty(keyText) || x.BlogName.IndexOf(keyText) >= 0 || x.BlogContent.IndexOf(keyText) >= 0)
                 .ToList();
                count = list.Count;
                list = list
                 .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                 .ToList();
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

            try
            {
                ViewBag.Images = Files_Service.GetByRefecense("" + b.BlogId).Where(x => x.FileType == "BLOG").ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Blogs/_List :", ex.Message, ex.ToString());
            }

            return PartialView(height);
        }
    }
}