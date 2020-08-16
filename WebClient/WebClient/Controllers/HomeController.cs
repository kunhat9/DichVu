using CORE.Tables;
using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebClient.AppSession;
using WebClient.Cookie;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class HomeController : BaseController
    {
        #region Login

        public ActionResult Login(string url)
        {
            ViewBag.url = url;
            if (Session[AppSessionKeys.USER_INFO] != null)
            {
                if (string.IsNullOrEmpty(url))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectPermanent(url);
                }
            }
            else
            {
                ViewBag.username = AppCookieInfo.UserID;
                ViewBag.password = AppCookieInfo.HashedPassword;

                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(string username, string password, string url, string remember)
        {
            ViewBag.url = url;

            if (Session[AppSessionKeys.USER_INFO] == null)
            {
                if (string.IsNullOrEmpty(username))
                {
                    ViewBag.error = "Chưa nhập Tên đăng nhập";
                    return View();
                }
                if (string.IsNullOrEmpty(password))
                {
                    ViewBag.error = "Chưa nhập Mật khẩu";
                    return View();
                }
                string newPass = GetMD5Hash(password); // pass MD5 

                VIEW_INFO_USER_LOGIN user = Users_Service.CheckLogin(username, password);
                if (user == null)
                {
                    ViewBag.error = "Đăng nhập sai hoặc bạn không có quyền vào";
                    return View();
                }
                else
                {
                    Session[AppSessionKeys.USER_INFO] = user;
                    if (remember == "on")
                    {
                        AppCookieInfo.UserID = username;
                        AppCookieInfo.HashedPassword = password;
                    }
                    else
                    {
                        AppCookieInfo.RemoveAllCookies();
                    }
                    //OK
                }
            }

            if (string.IsNullOrEmpty(url))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectPermanent(url);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        #endregion

        #region OutLogin

        public ActionResult Index()
        {
            try
            {
                List<TB_SERVICES> list = Services_Service.GetAll().Where(x=>x.ServiceStatus.Equals("A")).ToList();
                if (list == null) list = new List<TB_SERVICES>();

                List<TB_FILES> file = Files_Service.GetAll().Where(x => x.FileType == "SERVICE").ToList();
                ViewBag.Files = file;
                List<TB_FILES> fileSlider = Files_Service.GetAll().Where(x => x.FileType == "SLIDER").ToList();
                ViewBag.FileSlider = fileSlider;
                List<TB_SLIDERS> listSlider = new List<TB_SLIDERS>();
                listSlider = Slider_Service.GetAll().Take(5).ToList();
                ViewBag.Slider = listSlider;
                return View(list);
            }
            catch (Exception ex)
            {
                CORE.Helpers.IOHelper.WriteLog(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ADM"), GetIP(), ex.ToString());
            }

            return View();
        }

        [ChildActionOnly]
        public PartialViewResult _HomeHeader()
        {
            List<TB_TYPES> type = Types_Service.GetAll().Where(x => x.TypeStatus == "A").ToList();
            ViewBag.ServiceType = type;

            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult _HomeCategory()
        {
            List<TB_TYPES> types = Types_Service.GetAll().Where(x => x.TypeStatus == "A").ToList();

            return PartialView(types);
        }

        [ChildActionOnly]
        public PartialViewResult _HomeFooter()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult _HomeMenuLeft()
        {
            ViewBag.boxName = "Dịch vụ";
            ViewBag.fullName = "Đồ án";

            List<SYS_ACTIONS> actions = new List<SYS_ACTIONS>
            {
                new SYS_ACTIONS { Id = "0", Name = "Trang chủ", IsModule = false, IsRoot = true, IsShow = true, ControlPath = "/" }
            };
            List<TB_TYPES> type = Types_Service.GetAll();
            int i = 1;
            foreach (TB_TYPES item in type)
            {
                actions.Add(new SYS_ACTIONS { Id = "" + (++i), Name = item.TypeName, IsModule = false, IsRoot = true, ControlPath = "/Services/Detail?Id=" + item.TypeCode });
            }

            return PartialView(actions);
        }

        [ChildActionOnly]
        public PartialViewResult _Pagination(int maxNumber, int pageNumber)
        {
            ViewBag.maxNumber = maxNumber;
            ViewBag.pageNumber = pageNumber;
            return PartialView();
        }

        #endregion

        public ActionResult NotPermission()
        {
            return View();
        }

        public PartialViewResult _NotPermission()
        {
            return PartialView();
        }
    }
}