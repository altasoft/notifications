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
    /// User from external system, who will receive notification
    /// </summary>
    public class User : ModelBase
    {
        /// <summary>
        /// First name if available
        /// </summary>
        [StringLength(50)]
        public string FirstName { get; set; }
        /// <summary>
        /// Full name if available
        /// </summary>
        [StringLength(200)]
        public string FullName { get; set; }
        /// <summary>
        /// Mobile Number
        /// </summary>
        [StringLength(20)]
        public string MobileNumber { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [StringLength(100)]
        public string Email { get; set; }
        /// <summary>
        /// Id in External System, will be identified by this field, with application Id
        /// </summary>
        public int? ExternalUserId { get; set; }

        public int? ApplicationId { get; set; }
        public Application Application { get; set; }
    }
}
