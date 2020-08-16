using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAdmin.Controllers
{
    public class ReportsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Revenue(string startDate ="", string endDate ="")
        {
            List<V_Revenue> list = new List<V_Revenue>();
            try
            {
                list = Registers_Service.ReportChart("","",startDate,endDate);
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View(list);
        }
    }
}