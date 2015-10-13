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
    /// Event for subscribing UserGroup
    /// </summary>
    public class UserGroup : ModelBase
    {
        [Index("IX_Group_Application_Key", 1, IsUnique = true)]
        public int ApplicationId { get; set; }
        public Application Application { get; set; }

        [Index("IX_Group_Application_Key", 2, IsUnique = true), StringLength(50)]
        /// <summary>
        /// Will be identified by this field
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string Description { get; set; }

        public bool? IsSystem { get; set; }
    }
}
