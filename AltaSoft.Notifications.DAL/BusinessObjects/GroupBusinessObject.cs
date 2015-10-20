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
    public class GroupBusinessObject : BusinessObjectBase<Group>
    {
        public List<int> GetUserIdsByGroups(params int[] groupids)
        {
            return db.UserGroups.AsNoTracking().Where(x => groupids.Contains(x.GroupId)).Select(x => x.UserId).ToList();
        }

        public List<User> GetUsersByGroups(params int[] groupids)
        {
            return db.UserGroups.AsNoTracking().Where(x => groupids.Contains(x.GroupId)).Select(x => x.User).ToList();
        }

        public List<int> GetGroupIdsByKey(int applicationId, string key)
        {
            key = key.ToLower().Trim();

            var keys = key.Split(',');

            return db.Groups.AsNoTracking().Where(x => x.ApplicationId == applicationId && keys.Contains(x.Key.ToLower())).Select(x => x.Id).ToList();
        }
    }
}
