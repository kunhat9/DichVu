using CORE.Tables;
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
            DateTime? begin = new DateTime?();
            if (!string.IsNullOrEmpty(dtpBegin))
                begin = DateTime.ParseExact(dtpBegin, "dd/MM/yyyy", null);

            int count = 0;
            try
            {
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
    }
}