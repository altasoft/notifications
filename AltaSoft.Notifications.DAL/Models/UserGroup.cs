using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    /// <summary>
    /// Subscriptions for sending easily to groups of users
    /// </summary>
    public class UserGroup : ModelBase
    {
        [Index("IX_Subscription_UserGroup_User", 1, IsUnique = true)]
        public int GroupId { get; set; }
        public Group Group { get; set; }

        [Index("IX_Subscription_UserGroup_User", 2, IsUnique = true)]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
