using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    /// <summary>
    /// Notification providers, SendGrid, SMS, SignalR, etc.
    /// </summary>
    public class Provider : ModelBase
    {
        /// <summary>
        /// Will be identified by this field
        /// </summary>
        [Index("IX_Provider_Key", IsUnique = true), StringLength(20)]
        public string Key { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(2048)]
        public string WebUrl { get; set; }
    }
}
