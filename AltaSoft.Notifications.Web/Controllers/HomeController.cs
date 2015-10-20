using AltaSoft.Notifications.DAL;
using AltaSoft.Notifications.Web.Common;
using AltaSoft.Notifications.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AltaSoft.Notifications.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (UserContext.IsAuthenticated)
                return RedirectToAction("Info", "Account");



            using (var bo = new MessageBusinessObject())
            using (var bo2 = new SMSBusinessObject())
            {
                var fromDate = DateTime.Now.AddDays(-1);
                ViewBag.MessagesSentCount = bo.ItemsCount(x => x.RegDate >= fromDate && x.State == MessageStates.Success) + bo2.ItemsCount(x => x.RegDate >= fromDate && x.State == MessageStates.Success);
            }

            using (var bo = new ProviderBusinessObject())
            {
                ViewBag.ProvidersCount = bo.ItemsCount();
            }

            using (var bo = new ApplicationBusinessObject())
            {
                ViewBag.ApplicationsCount = bo.ItemsCount(x => !x.IsTestMode);
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}