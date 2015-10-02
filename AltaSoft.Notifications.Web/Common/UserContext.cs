using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltaSoft.Notifications.Web.Common
{
    public class UserContext : Singleton<UserContext>
    {
        public UserContext()
        {
            ApplicationId = 6;
        }


        public int? UserId { get; set; }
        public int? ApplicationId { get; set; }

    }
}