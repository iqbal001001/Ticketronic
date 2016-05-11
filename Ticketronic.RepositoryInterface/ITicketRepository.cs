using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketronic.Domain;

namespace Ticketronic.RepositoryInterface
{
    public interface ITicketRepository : IRepository<Ticket>
    {
    }
}
