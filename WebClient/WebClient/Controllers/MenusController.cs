using CORE.Tables;
using CORE.Views;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebClient.Controllers
{
    public class MenusController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _MenuGroup(int serviceId)
        {
            List<TB_MENUS> listMenu = new List<TB_MENUS>();
            try
            {
                listMenu = Menu_Service.GetByServiceId(serviceId);

                ViewBag.Height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);
            }
            catch (Exception)
            {

            }
            ViewBag.Menus = listMenu;

            return PartialView();
        }

        public PartialViewResult _MenuDetail(int menuId)
        {
            List<V_Group_Menu> listMenuDetails = new List<V_Group_Menu>();
            try
            {
                listMenuDetails = Menu_Service.GetAllDetails(menuId);
            }
            catch (Exception)
            {

            }
            ViewBag.Details = listMenuDetails;

            return PartialView();
        }
    }
}