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
        public async Task<List<Message>> GetListToBeProceeded(Expression<Func<Message, bool>> where)
        {
            var query = db.Set<Message>().Where(where);

            query = query.Where(x => x.State == MessageStates.Pending || x.State == MessageStates.ProviderManagerNotFound);
            query = query.Where(x => x.ProcessDate == null || x.ProcessDate <= DateTime.Now);

            query = query.Include(x => x.Provider);
            query = query.Include(x => x.User);
            query = query.Include(x => x.Application);

            var items = query.ToList();

            items.ForEach(x => x.State = MessageStates.Processing);

            await db.SaveChangesAsync();

            return items;
        }

        public List<Message> GetListWithUserAndProvider(Expression<Func<Message, bool>> where)
        {
            var query = this.GetListQuery(where).Include(x => x.User).Include(x => x.Provider);

            return query.ToList();
        }
    }
}
