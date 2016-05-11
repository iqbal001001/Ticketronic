using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketronic.Domain;
using Ticketronic.RepositoryInterface;

namespace Ticketronic.Data
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(IDbContextFactory contextFactory) : base(contextFactory) { }
    }
}
