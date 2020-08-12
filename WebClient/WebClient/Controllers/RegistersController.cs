using CORE.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebClient.Controllers
{
    public class RegistersController : BaseController
    {
        public ActionResult Index(int Id = 0)
        {
            TB_SERVICES d = Services_Service.GetById(Id);

            List<TB_SERVICES> list = Services_Service.GetAll().Where(x => x.ServiceStatus == "A").ToList();
            ViewBag.Selected = Id;

            return View(list);
        }
    }
}