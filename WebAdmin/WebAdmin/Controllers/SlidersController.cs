using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebAdmin.Controllers
{
    public class SlidersController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _DanhSach(string keyText = "", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_SLIDERS> list = new List<TB_SLIDERS>();

            int count = 0;
            try
            {
                keyText = keyText.Trim();
                list = Sliders_Service.GetAll()
                 .Where(x => string.IsNullOrEmpty(keyText) || x.SliderName.IndexOf(keyText) >= 0 || x.SliderContent.IndexOf(keyText) >= 0)
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

            return PartialView(list);
        }

        public PartialViewResult _ChiTiet(int sliderId = 0)
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_SLIDERS b = Sliders_Service.GetById(sliderId);
            ViewBag.Slider = b;

            try
            {
                ViewBag.Images = Files_Service.GetByRefecense("" + b.SliderId).Where(x => x.FileType == "SLIDER").ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Blogs/_List :", ex.Message, ex.ToString());
            }

            return PartialView(height);
        }
    }
}