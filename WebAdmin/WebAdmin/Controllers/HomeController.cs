using CORE.Tables;
using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAdmin.AppSession;
using WebAdmin.Cookie;

namespace WebAdmin.Controllers
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
            ViewBag.userName = "Dịch vụ";
            return PartialView();
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

                , new SYS_ACTIONS { Id = "11", Name = "Quản lý Đăng ký dịch vụ", IsModule = true, IsRoot = true, IsShow = true }
                , new SYS_ACTIONS { Id = "12", Name = "Danh sách Đăng ký dịch vụ", IsModule = false, IsRoot = false, IsShow = true
                    , ControlPath = "/Registers/Index", ParentId = "11" }

                , new SYS_ACTIONS { Id = "21", Name = "Quản lý Bài viết", IsModule = true, IsRoot = true, IsShow = true }
                , new SYS_ACTIONS { Id = "22", Name = "Danh sách Bài viết", IsModule = false, IsRoot = false, IsShow = true
                    , ControlPath = "/Blogs/Index", ParentId = "21" }

                //, new SYS_ACTIONS { Id = "31", Name = "Quản lý Slider", IsModule = true, IsRoot = true, IsShow = true }
                //, new SYS_ACTIONS { Id = "32", Name = "Danh sách Slider", IsModule = false, IsRoot = false, IsShow = true
                //    , ControlPath = "/Sliders/Index", ParentId = "31" }

                , new SYS_ACTIONS { Id = "41", Name = "Quản lý Dịch vụ", IsModule = true, IsRoot = true, IsShow = true }
                , new SYS_ACTIONS { Id = "42", Name = "Danh sách Loại dịch vụ", IsModule = false, IsRoot = false, IsShow = true
                    , ControlPath = "/Services/TypeList", ParentId = "41" }
                , new SYS_ACTIONS { Id = "43", Name = "Danh sách Dịch vụ", IsModule = false, IsRoot = false, IsShow = true
                    , ControlPath = "/Services/Index", ParentId = "41" }
                
                , new SYS_ACTIONS { Id = "44", Name = "Danh sách Thực đơn", IsModule = false, IsRoot = false, IsShow = true
                    , ControlPath = "/Services/MenuList", ParentId = "41" }
         

                , new SYS_ACTIONS { Id = "51", Name = "Quản lý khuyến mại", IsModule = true, IsRoot = true, IsShow = true }
                , new SYS_ACTIONS { Id = "52", Name = "Danh sách khuyến mại", IsModule = false, IsRoot = false, IsShow = true
                    , ControlPath = "/Vouchers/Index", ParentId = "51" }

                , new SYS_ACTIONS { Id = "61", Name = "Quản lý người dùng", IsModule = true, IsRoot = true, IsShow = true }
                , new SYS_ACTIONS { Id = "62", Name = "Danh sách người dùng", IsModule = false, IsRoot = false, IsShow = true
                    , ControlPath = "/User/Index", ParentId = "61" }

                , new SYS_ACTIONS { Id = "71", Name = "Báo cáo - thống kê", IsModule = true, IsRoot = true, IsShow = true }
                , new SYS_ACTIONS { Id = "72", Name = "Báo cáo doanh thu", IsModule = false, IsRoot = false, IsShow = true
                    , ControlPath = "/Reports/Revenue", ParentId = "71" }
            };
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