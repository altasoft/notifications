using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [StringLength(50)]
        public string SecretKey { get; set; }

        /// <summary>
        /// for login in Administration Portal
        /// </summary>
        [StringLength(20)]
        public string Username { get; set; }

        /// <summary>
        /// for login in Administration Portal
        /// </summary>
        [StringLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// for SignalR provider, to get user id, based on Token and IPAddress
        /// </summary>
        [StringLength(2048)]
        public string CheckUserIdUrl { get; set; }

        [StringLength(50)]
        public string EmailFromAddress { get; set; }

        [StringLength(100)]
        public string EmailFromFullName { get; set; }


        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(2048)]
        public string ImageUrl { get; set; }

        [StringLength(10)]
        public string SMSSenderNumber { get; set; }

        public bool IsTestMode { get; set; }
    }
}
