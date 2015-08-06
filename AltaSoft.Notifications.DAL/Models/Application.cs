using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    /// <summary>
    /// Web portals, or applications, that will use Notifications System
    /// </summary>
    public class Application : ModelBase
    {
        /// <summary>
        /// for API authentification
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// for login in Administration Portal
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// for login in Administration Portal
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// for SignalR provider, to get user id, based on Token and IPAddress
        /// </summary>
        public string CheckUserIdUrl { get; set; }

        public string EmailFromAddress { get; set; }

        public string EmailFromFullName { get; set; }


        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsTestMode { get; set; }
    }
}
