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
    public class UserBusinessObject : BusinessObjectBase<User>
    {
        public User GetByExternalUserId(int applicationId, int externalUserId)
        {
            return db.Users.AsNoTracking().FirstOrDefault(x => x.ApplicationId == applicationId && x.ExternalUserId == externalUserId);
        }

        public void Save(User user)
        {
            var existingUser = this.GetList(x => x.ApplicationId == user.ApplicationId && x.ExternalUserId == user.ExternalUserId).FirstOrDefault();
            if (existingUser == null)
            {
                Create(user);
                return;
            }


            user.Id = existingUser.Id;
            user.RegDate = existingUser.RegDate;

            Update(user);
        }
    }
}
