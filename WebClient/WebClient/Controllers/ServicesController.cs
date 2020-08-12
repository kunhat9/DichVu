using CORE.Tables;
using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebClient.Controllers
{
    public class ServicesController : BaseController
    {
        public ActionResult Index()
        {
            List<TB_SERVICES> list = Services_Service.GetAll().Where(x => x.ServiceStatus == "A").ToList();

            List<TB_FILES> file = Files_Service.GetAll().Where(x => x.FileType == "SERVICE").ToList();
            ViewBag.Files = file;

            return View(list);
        }

        public ActionResult Detail(int Id = 0)
        {
            ViewBag.Height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);
            TB_SERVICES d = Services_Service.GetById(Id);

            List<TB_TYPE_DETAILS> types = Types_Service.GetAllDetails(d.ServiceTypeCode);
            ViewBag.Types = types;
            List<TB_SERVICE_DETAILS> details = Services_Service.GetAllDetails(Id);
            ViewBag.Details = details;

            List<TB_FILES> file = Files_Service.GetByRefecense("" + Id).Where(x => x.FileType == "SERVICE").ToList();
            List<TB_TYPES> listType = Types_Service.GetAll();
            ViewBag.Files = file;
            ViewBag.ListType = listType;

            List<TB_MENUS> listMenu = new List<TB_MENUS>();
            listMenu = Menu_Service.GetByServiceId(Id);
            ViewBag.Menus = listMenu;

            return View(d);
        }

        public ActionResult Search(string group = "", string key = "")
        {
            List<TB_TYPES> types = Types_Service.GetAll()
                .Where(x => x.TypeStatus == "A" && (string.IsNullOrEmpty(group) || x.TypeCode == group)).ToList();
            ViewBag.Types = types;

            List<TB_SERVICES> services = Services_Service.GetAll()
                .Where(x => x.ServiceStatus == "A" && (string.IsNullOrEmpty(key) || x.ServiceName.IndexOf(key) > -1)
                    && types.Select(y => y.TypeCode).Contains(x.ServiceTypeCode)).ToList();
            ViewBag.Services = services;

            ViewBag.Group = group;

            List<TB_FILES> file = Files_Service.GetAll().Where(x => x.FileType == "SERVICE").ToList();
            if (file == null) file = new List<TB_FILES>();
            ViewBag.Files = file;

            return View();
        }
    }
}