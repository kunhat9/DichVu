using CORE.Tables;
using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAdmin.Controllers
{
    public class RegistersController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _DanhSach(string keyText = "", string dtpBegin = "", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_REGISTERS> list = new List<TB_REGISTERS>();
            List<TB_SERVICES> listService = new List<TB_SERVICES>();
            DateTime? begin = new DateTime?();
            if (!string.IsNullOrEmpty(dtpBegin))
                begin = DateTime.ParseExact(dtpBegin, "dd/MM/yyyy", null);

            int count = 0;
            try
            {
                listService = Services_Service.GetAll();
                keyText = keyText.Trim().ToLower();
                list = Registers_Service.GetAll()
                    .Where(x => (string.IsNullOrEmpty(keyText)
                        || x.RegisterUserName.ToLower().IndexOf(keyText) >= 0
                        || x.RegisterUserEmail.ToLower().IndexOf(keyText) >= 0
                        || x.RegisterUserPhone.ToLower().IndexOf(keyText) >= 0)
                        && (!begin.HasValue
                        || x.RegisterDateBegin >= begin.Value))
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
            ViewBag.Service = listService;
            ViewBag.maxNumber = Math.Ceiling((double)count / pageSize);
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;

            return PartialView(list);
        }

        public PartialViewResult _ChiTiet(int registerId = 0)
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_REGISTERS r = Registers_Service.GetById(registerId);
            ViewBag.Register = r;
            
            return PartialView(height);
        }
        public PartialViewResult _MenuListDetail(int registerId=0)
        {
            
            TB_REGISTERS regis = Registers_Service.GetById(registerId);
            int serviceId = regis.RegisterServiceId;
            int menuId = regis.RegisterMenuId;
            
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);

            TB_MENUS menu = Menus_Service.GetById(menuId);
            if (menu == null)
            {
                menu = new TB_MENUS();
            }
            menu.MenuServiceId = serviceId;
            ViewBag.Menu = menu;

            List<V_Group_Menu> details = Menus_Service.GetAllDetails(menuId);
            List<TB_REGISTER_DETAILS> regisDetail = Registers_Service.GetByRegisterId(registerId);
            List<V_Group_Menu> list = new List<V_Group_Menu>();
            foreach (var item in details)
            {
                V_Group_Menu v = new V_Group_Menu();
                v = item;
            
                foreach (var data in item.MgroupDetail)
                {
                    if(regisDetail.Where(x=>x.RdetailMdetailId == data.MdetailId).ToList().Count >0)
                    {
                        v.MgroupDetail = new List<V_Details_Menu>();
                        v.MgroupDetail.Add(data);
                        list.Add(v);
                        break;
                    }
                }
            }

            ViewBag.Details = list;

            return PartialView(height);
        }
    }
}