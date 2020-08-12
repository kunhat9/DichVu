using CORE.Tables;
using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebAdmin.Controllers
{
    public class ServicesController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _List(string keyText = "", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_SERVICES> list = new List<TB_SERVICES>();

            int count = 0;
            try
            {
                keyText = keyText.Trim();
                list = Services_Service.GetAll()
                 .Where(x => string.IsNullOrEmpty(keyText) || x.ServiceName.IndexOf(keyText) >= 0 || x.ServiceContent.IndexOf(keyText) >= 0)
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

        public PartialViewResult _ListDetail(int serviceId = 0)
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_SERVICES s = Services_Service.GetById(serviceId);
            ViewBag.Service = s;

            List<TB_TYPES> list = Types_Service.GetAll();
            ViewBag.TypeList = list;

            try
            {
                ViewBag.Images = Files_Service.GetByRefecense("" + s.ServiceId).Where(x => x.FileType == "SERVICE").ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Blogs/_List :", ex.Message, ex.ToString());
            }

            return PartialView(height);
        }

        public PartialViewResult _ServiceDetail(string typeCode = "", int serviceId = 0)
        {
            TB_SERVICES s = Services_Service.GetById(serviceId);
            if (s == null) s = new TB_SERVICES();

            List<TB_TYPE_DETAILS> details = Types_Service.GetAllDetails(typeCode);
            ViewBag.Details = details;

            List<TB_SERVICE_DETAILS> sDetails = Service_Details_Service.GetAllByServiceId(s.ServiceId);
            ViewBag.SDetails = sDetails;

            return PartialView();
        }

        public ActionResult MenuDetail(int serviceId = 0)
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_SERVICES s = Services_Service.GetById(serviceId);
            ViewBag.Service = s;

            List<TB_TYPES> list = Types_Service.GetAll();
            ViewBag.TypeList = list;

            try
            {
                ViewBag.Images = Files_Service.GetByRefecense("" + s.ServiceId).Where(x => x.FileType == "SERVICE").ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Blogs/_List :", ex.Message, ex.ToString());
            }

            return PartialView(height);
        }

        public ActionResult TypeList()
        {
            return View();
        }

        public PartialViewResult _TypeList(string keyText = "", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_TYPES> list = new List<TB_TYPES>();

            int count = 0;
            try
            {
                keyText = keyText.Trim();
                list = Types_Service.GetAll()
                 .Where(x => string.IsNullOrEmpty(keyText) || x.TypeCode.IndexOf(keyText) >= 0 || x.TypeName.IndexOf(keyText) >= 0)
                 .ToList();
                count = list.Count;
                list = list
                 .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                 .ToList();
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Services/_TypeList :", ex.Message, ex.ToString());
            }

            ViewBag.maxNumber = Math.Ceiling((double)count / pageSize);
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;

            return PartialView(list);
        }

        public PartialViewResult _TypeListDetail(string typeCode = "")
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_TYPES type = Types_Service.GetById(typeCode);
            ViewBag.ServiceType = type;

            List<TB_TYPE_DETAILS> details = Types_Service.GetAllDetails(typeCode);
            ViewBag.Details = details;

            return PartialView(height);
        }

        public ActionResult MenuList(int serviceId = 0)
        {
            List<string> t = Types_Service.GetAll()
                .Where(x => x.TypeType == "SK" && x.TypeStatus == "A")
                .Select(x => x.TypeCode).ToList();
            List<TB_SERVICES> s = Services_Service.GetAll()
                .Where(x => t.Contains(x.ServiceTypeCode) && x.ServiceStatus == "A")
                .ToList();
            ViewBag.ServiceList = s;

            ViewBag.Selected = serviceId;

            return View();
        }

        public PartialViewResult _MenuList(int serviceId = 0)
        {
            List<TB_MENUS> list = new List<TB_MENUS>();

            try
            {
                list = Menus_Service.GetByServiceId(serviceId);
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Services/_TypeList :", ex.Message, ex.ToString());
            }

            return PartialView(list);
        }

        public PartialViewResult _MenuListDetail(int serviceId, int menuId = 0)
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_MENUS menu = Menus_Service.GetById(menuId);
            if (menu == null)
            {
                menu = new TB_MENUS();
            }
            menu.MenuServiceId = serviceId;
            ViewBag.Menu = menu;

            List<V_Group_Menu> details = Menus_Service.GetAllDetails(menuId);
            ViewBag.Details = details;

            return PartialView(height);
        }
    }
}