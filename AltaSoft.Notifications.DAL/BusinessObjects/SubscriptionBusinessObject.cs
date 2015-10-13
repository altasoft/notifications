using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;

namespace AltaSoft.Notifications.DAL
{
    public class SubscriptionBusinessObject : BusinessObjectBase<Subscription>
    {
        public void Save(Subscription entity)
        {
            var existing = Get(entity.EventId, entity.UserId, entity.ProviderId);
            if (existing != null)
                return;

            this.Create(entity);
        }

        public void Unsubscribe(Subscription entity)
        {
            var existing = Get(entity.EventId, entity.UserId, entity.ProviderId);
            if (existing != null)
                return;

            this.Delete(existing);
        }

        public override List<Subscription> GetList(Expression<Func<Subscription, bool>> where = null)
        {
            var query = this.GetListQuery(where);

            query = query.Include(x => x.User);

            return query.ToList();
        }

        public List<Subscription> GetList(int applicationId, List<string> eventKeys, List<string> providerKeys, List<string> externalUserIds)
        {
            var eventIds = db.Events.Where(x => x.ApplicationId == applicationId && eventKeys.Contains(x.Key)).Select(x => x.Id).ToList();
            var userIds = db.Users.Where(x => x.ApplicationId == applicationId && externalUserIds.Contains(x.ExternalUserId)).Select(x => x.Id).ToList();
            var providerIds = db.Providers.Where(x => providerKeys.Contains(x.Key)).Select(x => x.Id).ToList();


            IQueryable<Subscription> query = db.Subscriptions.AsNoTracking();

            if (eventIds.Count > 0)
                query = query.Where(x => eventIds.Contains(x.EventId));

            if (userIds.Count > 0)
                query = query.Where(x => userIds.Contains(x.UserId));

            if (providerIds.Count > 0)
                query = query.Where(x => providerIds.Contains(x.ProviderId));


            query = query.Include(x => x.Provider);
            query = query.Include(x => x.Event);
            query = query.Include(x => x.User);


            return query.ToList();
        }


        protected Subscription Get(int eventId, int userId, int providerId)
        {
            var result = db.Subscriptions.AsNoTracking().FirstOrDefault(x => x.EventId == eventId && x.UserId == userId && x.ProviderId == providerId);

            return result;
        }


        public void Save(int eventId, List<int> userIds, bool clearBeforeAdd = true)
        {
            if (clearBeforeAdd)
            {
                var removeItems = db.Subscriptions.Where(x => x.EventId == eventId);
                db.Subscriptions.RemoveRange(removeItems);
                db.SaveChanges();
            }

            if (userIds != null)
            {
                var existingItems = db.Subscriptions.AsNoTracking().Where(x => x.EventId == eventId).ToList();
                existingItems.ForEach(x => userIds.Remove(x.UserId));

                db.Subscriptions.AddRange(userIds.Select(x =>
                    new Subscription
                    {
                        EventId = eventId,
                        UserId = x,
                        RegDate = DateTime.Now
                    })
                );

                db.SaveChanges();
            }
        }
    }
}
