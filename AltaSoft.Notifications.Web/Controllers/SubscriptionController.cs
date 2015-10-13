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
    public class SubscriptionController : Controller
    {
        public virtual ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Create(SubscriptionsModel model)
        {
            try
            {
                //using (var manager = new SubscriptionBusinessObject())
                //{
                //    manager.Save(model.EventId.Value, model.UserIds, false);
                //    ModelState.Clear();
                //    TempData["STATUS_MESSAGE"] = "Success";
                //    model = new SubscriptionsModel();
                //}
            }
            catch (Exception ex)
            {
                TempData["SA_MODELSTATE"] = ModelState;
            }

            return RedirectToAction("List");
        }


        public virtual ActionResult Edit(int? id)
        {
            var model = GetItemById(id);
            if (model == null)
                return RedirectToAction("List");

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SubscriptionsModel model)
        {
            try
            {
                //using (var manager = new SubscriptionBusinessObject())
                //{
                //    manager.Save(model.EventId.Value, model.UserIds);
                //    ModelState.Clear();
                //    TempData["STATUS_MESSAGE"] = "Success";
                //}
            }
            catch (Exception ex)
            {
                TempData["SA_MODELSTATE"] = ModelState;
            }

            return RedirectToAction("List");
        }

        public virtual ActionResult List()
        {
            var model = GetItems();

            ViewBag.ActivePage = 1;
            ViewBag.ItemsOnPage = Int32.MaxValue;
            ViewBag.PagesCount = 1;

            return View(model);
        }


        List<SubscriptionsModel> GetItems()
        {
            //using (var manager = new SubscriptionBusinessObject())
            //{
            //    var items = manager.GetList(null);

            //    var model = items.GroupBy(x => x.EventId).Select(x => new SubscriptionsModel
            //    {
            //        EventId = x.Key,
            //        UserIds = x.Select(y => y.UserId).ToList()
            //    })
            //    .ToList();

            //    using (var userBO = new UserBusinessObject())
            //    using (var eventBO = new EventBusinessObject())
            //    {
            //        model.ForEach(x =>
            //        {
            //            var names = userBO.GetList(y => x.UserIds.Contains(y.Id)).Select(z => z.FullName).ToList();
            //            var eventObj = eventBO.GetById(x.EventId);

            //            x.EventDescription = eventObj == null ? x.EventId.ToString() : eventObj.Description;
            //            x.UserFullNames = names;
            //        });
            //    }

            //    return model.OrderBy(x => x.EventId).ToList();
            //}

            return null;
        }

        SubscriptionsModel GetItemById(int? id)
        {
            //using (var manager = new SubscriptionBusinessObject())
            //{
            //    var items = manager.GetList(x => x.EventId == id);

            //    var model = new SubscriptionsModel
            //    {
            //        EventId = id,
            //        UserIds = items.Select(x => x.UserId).ToList()
            //    };

            //    return model;
            //}

            return null;
        }
    }
}