using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebAdmin.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _DanhSach(string keyText = "", string type = "", string status = "A", int pageNumber = 1, int pageSize = 10)
        {
            List<TB_USERS> list = new List<TB_USERS>();

            int count = 0;
            try
            {
                keyText = keyText.Trim();
                list = Users_Service.GetAll()
                 .Where(x => (string.IsNullOrEmpty(keyText) || x.Username.IndexOf(keyText) >= 0 || x.UserPhone.IndexOf(keyText) >= 0 || x.UserAddress.IndexOf(keyText) >= 0 || x.UserNote.IndexOf(keyText) >= 0 || x.UserEmail.IndexOf(keyText) >= 0 || x.UserFullName.IndexOf(keyText) >= 0)
                 && x.UserStatus.Equals(status)
                 && (string.IsNullOrEmpty(type) ? true : x.UserType.Equals(type))
                 )
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

        public PartialViewResult _ChiTiet(int userId = 0)
        {
            int height = (int)(Request.Browser.ScreenPixelsHeight * 0.85);
            TB_USERS b = new TB_USERS();

            try
            {
                b = Users_Service.GetById(userId);
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(StartUpPath, IpAddress, "Blogs/_List :", ex.Message, ex.ToString());
            }
            ViewBag.Slider = b;
            return PartialView(height);
        }
    }
}