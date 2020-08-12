using CORE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAdmin.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Revenue()
        {
            List<V_Revenue> list = new List<V_Revenue>();
            list.Add(new V_Revenue { sMonth = 1, Cost = 100000, Revenue = 200000 });
            list.Add(new V_Revenue { sMonth = 2, Cost = 200000, Revenue = 100000 });
            list.Add(new V_Revenue { sMonth = 3, Cost = 100000, Revenue = 300000 });
            list.Add(new V_Revenue { sMonth = 4, Cost = 150000, Revenue = 150000 });
            return View(list);
        }
    }
}