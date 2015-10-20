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
    public class SMSBusinessObject : BusinessObjectBase<SMS>
    {
        public async Task<List<SMS>> GetListToBeProceeded(int? priority)
        {
            var items = db.Database.SqlQuery<SMS>("EXEC dbo.SMSSelectToProcceed {0}", priority).ToList();

            return items;
        }

        public void UpdateMessageStatusToSuccess(int id, double durationInMS)
        {
            db.Database.ExecuteSqlCommand("EXEC dbo.SMSStatusUpdate @Id={0}, @IsSuccess={1}, @ProcessingDuration={2}", id, 1, Convert.ToInt32(durationInMS));
        }

        public void UpdateMessageStatusToFailed(int id, double durationInMS, string errorCode, string errorMessage)
        {
            db.Database.ExecuteSqlCommand("EXEC dbo.SMSStatusUpdate @Id={0}, @IsSuccess={1}, @ProcessingDuration={2}, @ErrorCode={3}, @ErrorDescription={4}", id, 0, Convert.ToInt32(durationInMS), errorCode, errorMessage);
        }

        public new int Create(SMS item)
        {
            var id = db.Database.SqlQuery<int>("EXEC dbo.SMSAddInternal @application_id={0}, @application_product_id={1}, @external_id={2}, @provider_id={3}, @state={4}, @priority={5}, @to={6}, @message={7}, @sender_number={8}, @sleep_from_time={9}, @sleep_to_time={10}, @process_from_date={11}, @process_to_date={12}, @group_id={13}, @is_test={14}",
                item.ApplicationId,
                item.ApplicationProductId,
                item.ExternalId,
                item.ProviderId,
                item.State,
                item.Priority,
                item.To,
                item.Message,
                item.SenderNumber,
                item.SleepFromTime,
                item.SleepToTime,
                item.ProcessDate,
                item.ProcessDateDeadline,
                item.GroupId,
                item.IsTest
            ).FirstOrDefault();

            return id;
        }
    }
}
