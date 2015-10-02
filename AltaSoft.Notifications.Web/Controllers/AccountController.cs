using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GDBS.UI.Controllers
{
    public partial class AccountController : Controller
    {
        private const string Owner = "Default";
        private const string Bucket = "client-attachments";


        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //[HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        //public virtual ActionResult Login(LoginViewModel model, string returnUrl)
        //{
        //    try
        //    {
        //        ViewBag.ReturnUrl = returnUrl;

        //        if (ModelState.IsValid)
        //        {
        //            using (var userBO = new UserBusinessObject(Utility.Constants.SystemUserId))
        //            {
        //                var result = userBO.Login(model.UserName, model.Password, GetCurrentIpAddress(), GDBS.Models.System.UserSessionSources.WebPortal, Request.UserAgent);

        //                UserContext.SignIn(result.Value);

        //                userBO.LogUserAuth(result.Key, null, model.UserName, Altasoft.Infrastructure.Utility.CryptographyHelper.SHA1HashData(model.Password), GDBS.Models.Logging.UserAuthTypes.SuccessfulLogin, null);

        //                // todo: for del - for debug
        //                if (result.Key <= 0)
        //                    userBO.LogUserAuth(result.Key, null, model.UserName, Altasoft.Infrastructure.Utility.CryptographyHelper.SHA1HashData(model.Password), GDBS.Models.Logging.UserAuthTypes.SuccessfulLogin, "Debug: Warning");

        //                Session["SkipCheck"] = false;
        //            }

        //            if (string.IsNullOrEmpty(returnUrl))
        //                returnUrl = Url.Action("Index", "Home");

        //            return RedirectToLocal(returnUrl);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.MapToModelState<LoginViewModel>(ModelState);

        //        string error = ex.Message;

        //        if (ex.InnerException != null)
        //            error += ". " + ex.InnerException.Message;

        //        using (var userBO = new UserBusinessObject(Utility.Constants.SystemUserId))
        //            userBO.LogUserAuth(null, null, model.UserName, Altasoft.Infrastructure.Utility.CryptographyHelper.SHA1HashData(model.Password), GDBS.Models.Logging.UserAuthTypes.FailedLogin, error);
        //    }

        //    return View(model);
        //}

        //public virtual ActionResult LogOff()
        //{
        //    UserContext.SignOut();

        //    return RedirectToAction("Index", "Home");
        //}

        public virtual ActionResult IPInfos()
        {
            var serverVariables = new List<string>();

            foreach (var item in Request.ServerVariables)
                foreach (var item2 in Request.ServerVariables.GetValues(item.ToString()))
                {
                    serverVariables.Add(string.Format("{0}={1}", item, item2));
                }


            return Json(new
            {
                ServerVariables = serverVariables,
                HTTP_X_FORWARDED_FOR = Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                REMOTE_ADDR = Request.ServerVariables["REMOTE_ADDR"]
            }, JsonRequestBehavior.AllowGet);
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