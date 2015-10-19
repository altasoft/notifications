using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AltaSoft.Notifications.DAL;
using AltaSoft.Notifications.DAL.Context;
using AltaSoft.Notifications.Web.Models;
using AltaSoft.Notifications.Web.Common;
using System.IO;

namespace AltaSoft.Notifications.Web.Controllers
{
    public class MessageController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: Message
        public ActionResult Index(string id)
        {
            int i;
            var messageIds = new List<int>();

            if (!String.IsNullOrEmpty(id))
            {
                id.Split(',').ToList().ForEach(x =>
                {
                    if (Int32.TryParse(x, out i))
                        messageIds.Add(i);
                });
            }

            using (var bo = new MessageBusinessObject())
            {
                var model = bo.GetList(x => messageIds.Count == 0 || messageIds.Contains(x.Id));

                return View(model);
            }
        }

        // GET: Message/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }


        public ActionResult Compose()
        {
            var model = new ComposeModel();

            return View(model);
        }
        [HttpPost]
        public ActionResult Compose(ComposeModel model)
        {
            try
            {
                if ((model.Provider == 1 || model.Provider == 4) && String.IsNullOrEmpty(model.Subject))
                    throw new Exception("Please fill - Subject");


                if (!String.IsNullOrWhiteSpace(model.FileContent)) {
                    model.Message = model.FileContent;
                }

                if (String.IsNullOrEmpty(model.Message))
                    throw new Exception("Please fill - Message");

                if (model.Users == null)
                    model.Users = new List<int>();

                if (model.Events != null && model.Events.Count > 0)
                {
                    //using (var bo = new SubscriptionBusinessObject())
                    //{
                    //    var eventUserIds = bo.GetList(x => model.Events.Contains(x.EventId) && x.ProviderId == model.Provider).Select(x => x.UserId).ToList();
                    //    model.Users.AddRange(eventUserIds);

                    //    model.Users = model.Users.Distinct().ToList();
                    //}
                }

                var users = new List<DAL.User>();

                using (var bo = new UserBusinessObject())
                    users = bo.GetList(x => model.Users.Contains(x.Id));

                if (users.Count == 0)
                    throw new Exception("Users must be selected");



                var applicationId = UserContext.Current.Id.Value;
                var resultIds = new List<int>();

                using (var bo = new MessageBusinessObject())
                {
                    var groupId = Guid.NewGuid();

                    users.ForEach(u =>
                    {
                        var message = new Message
                        {
                            UserId = u.Id,
                            To = Helper.GetToByProvider(u, model.Provider),
                            ProviderId = model.Provider,
                            ApplicationId = applicationId,
                            Subject = model.Subject,
                            Content = model.Message,
                            ProcessDate = DateTime.Now,
                            GroupId = groupId,
                            Priority = MessagePriority.Normal
                        };

                        resultIds.Add(bo.Create(message));
                    });
                }


                return Json(new
                {
                    IsSuccess = true,
                    MessageIds = resultIds
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Error = ex.Message,
                    ErrorDetails = ex.ToString()
                },
                JsonRequestBehavior.AllowGet);
            }
        }


        // GET: Message/Create
        public ActionResult Create()
        {
            ViewBag.ProviderId = new SelectList(db.Providers, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,ProviderId,Subject,Content,RetryCount,ErrorMessage,ErrorDetails,State,Priority,ProcessDate,ProcessingDuration,RegDate,LastUpdateDate,RowVersion")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProviderId = new SelectList(db.Providers, "Id", "Name", message.ProviderId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", message.UserId);
            return View(message);
        }

        // GET: Message/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProviderId = new SelectList(db.Providers, "Id", "Name", message.ProviderId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", message.UserId);
            return View(message);
        }

        // POST: Message/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,ProviderId,Subject,Content,RetryCount,ErrorMessage,ErrorDetails,State,Priority,ProcessDate,ProcessingDuration,RegDate,LastUpdateDate,RowVersion")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProviderId = new SelectList(db.Providers, "Id", "Name", message.ProviderId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", message.UserId);
            return View(message);
        }

        // GET: Message/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
