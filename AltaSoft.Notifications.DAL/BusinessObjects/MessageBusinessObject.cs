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
    public class MessageBusinessObject : BusinessObjectBase<Message>
    {
        public async Task<List<Message>> GetListToBeProceeded(MessagePriority priority)
        {
            var items = db.Database.SqlQuery<Message>("EXEC dbo.MessagesSelectToProcceed {0}", (int)priority).ToList();

            return items;
        }

        public List<Message> GetListWithUserAndProvider(Expression<Func<Message, bool>> where)
        {
            var query = this.GetListQuery(where).Include(x => x.User).Include(x => x.Provider);

            return query.ToList();
        }

        public void LoadReferences(Message item)
        {
            db.Messages.Attach(item);
            db.Entry(item).Reference(x => x.Provider).Load();
            db.Entry(item).Reference(x => x.User).Load();
            db.Entry(item).Reference(x => x.Application).Load();
        }
    }
}
