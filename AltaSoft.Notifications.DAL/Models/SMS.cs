using AltaSoft.Notifications.DAL.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltaSoft.Notifications.DAL
{
    public class SMS : ModelBase, IProviderInfo
    {
        /// <summary>
        /// თუ რომელი პროექტისთვის მოხდა გაგზავნა
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// კონკრეტულად რომელი პროდუქტისთვის მოხდა გაგზავნა, 
        /// </summary>
        public int? ApplicationProductId { get; set; }
        /// <summary>
        /// გამგზავნი სისტემის იდენთიფიკატორი, არააუცილებელი ველი, საჭიროების შემთხვევაში შეუძლია გამომძახებელს გამოიყენოს
        /// </summary>
        public int? ExternalSystemId { get; set; }
        /// <summary>
        /// მაგთი, ჯეოსელი და ა.შ.
        /// </summary>
        public int ProviderId { get; set; }

        public MessageStates State { get; set; }

        public int Priority { get; set; }


        [StringLength(20)]
        public string To { get; set; }
        [StringLength(160)]
        public string Message { get; set; }
        [StringLength(20)]
        public string SenderNumber { get; set; }
        public DateTime? SleepFromTime { get; set; }
        public DateTime? SleepToTime { get; set; }

        [StringLength(20)]
        public string ErrorCode { get; set; }
        [StringLength(4000)]
        public string ErrorDescription { get; set; }

        public int ProcessingDuration { get; set; }

        public DateTime? ProcessDate { get; set; }
        public DateTime? ProcessDateDeadline { get; set; }

        public int TryCount { get; set; }

        public Guid? GroupId { get; set; }

        public bool ForceSendingNow { get; set; }


        //TODO: გასარკვევი
        public int TTL { get; set; }
        [StringLength(50)]
        public string TrackMessageId { get; set; }
        [StringLength(10)]
        public string TrackMessageStatus { get; set; }

        public bool IsTest { get; set; }
    }
}
