using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketronic.Domain;
using Ticketronic.RepositoryInterface;

namespace Ticketronic.Data
{
    public class TransactionRepository  : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDbContextFactory contextFactory) : base(contextFactory) { }
    }

}
