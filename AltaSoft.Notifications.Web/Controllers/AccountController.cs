using AltaSoft.Notifications.DAL;
using AltaSoft.Notifications.Web.Common;
using AltaSoft.Notifications.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace GDBS.UI.Controllers
{
    public partial class AccountController : Controller
    {
        private const string Owner = "Default";
        private const string Bucket = "client-attachments";


        public virtual ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public virtual ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;

                if (ModelState.IsValid)
                {
                    using (var appBO = new ApplicationBusinessObject())
                    {
                        var user = appBO.Authenticate(model.Username, model.Password);
                        if (user == null)
                            throw new Exception("Invalid Credentials");

                        UserContext.Current.Login(user.Id, user.Name);
                    }

                    return RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;

                if (ex.InnerException != null)
                    error += ". " + ex.InnerException.Message;

                ViewBag.Error = error;
            }

            return View(model);
        }

        public virtual ActionResult LogOff()
        {
            UserContext.Current.Logout();

            return RedirectToAction("Index", "Home");
        }

        [Secured]
        public virtual ActionResult Info()
        {
            using (var bo = new MessageBusinessObject())
            {
                var fromDate = DateTime.Now.AddDays(-1);
                ViewBag.SMSSentCount = bo.ItemsCount(x => x.ProviderId == 2 && x.RegDate.Month == DateTime.Now.Month && x.ApplicationId == UserContext.Current.Id && x.State == MessageStates.Success);
                ViewBag.EmailSentCount = bo.ItemsCount(x => x.ProviderId == 4 && x.RegDate.Month == DateTime.Now.Month && x.ApplicationId == UserContext.Current.Id && x.State == MessageStates.Success);
            }


            return View();
        }

        ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        string GetCurrentIpAddress()
        {
            //var xff = Request.Headers.AllKeys
            //    .Where(x => x == "X-Forwarded-For")
            //    .Select(k => Request.Headers[k])
            //    .FirstOrDefault();

            //if (Request != null)
            //    if (xff != null)
            //        return xff;
            //    else
            //        if (Request.UserHostAddress != null)
            //            return Request.UserHostAddress;
            //        else
            //            return string.Empty;
            //else
            //    return string.Empty;


            var ip = (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                ? Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                : Request.ServerVariables["REMOTE_ADDR"];

            if (ip.Contains(","))
                ip = ip.Split(',').First().Trim();

            return ip;

        }
    }
}