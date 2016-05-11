using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketronic.Data;
using Ticketronic.RepositoryInterface;

namespace Ticketronic.Data
{
    public class TicketronicUnitOfWork : IUnitOfWork
    {
        private IDbContextFactory _contextFactory;
        private TicketronicDBContext _context;

        public TicketronicUnitOfWork(IDbContextFactory contextFactory)
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException("contextFactory");
            }

            _contextFactory = contextFactory;
        }

        protected TicketronicDBContext Context
        {
            get { return _context ?? (_context = _contextFactory.Get()); }
        }

        public void SaveChanges()
        {
            //Context.Commit();
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
