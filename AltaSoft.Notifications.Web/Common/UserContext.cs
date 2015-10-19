using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltaSoft.Notifications.Web.Common
{
    public class UserContext : Singleton<UserContext>
    {
        public static bool IsAuthenticated
        {
            get { return UserContext.Current.Id != null; }
        }

        public int? Id { get; set; }
        public string Name { get; set; }


        public UserContext()
        {
        }


        public void Login(int? applicationId, string name)
        {
            Id = applicationId;
            Name = name;
        }

        public void Logout()
        {
            Id = null;
        }
    }
}