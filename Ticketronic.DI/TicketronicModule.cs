using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Ticketronic.Data;
using Ticketronic.RepositoryInterface;

namespace Ticketronic.DI
{
    public class TicketronicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<TicketronicDBContextFactory>()
                .As<IDbContextFactory>()
                .InstancePerRequest();

            builder
                .RegisterType<TicketronicUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();
            
            builder
                .RegisterType<EventRepository>()
                .As<IEventRepository>()
                .InstancePerRequest();

            builder
               .RegisterType<TicketRepository>()
               .As<ITicketRepository>()
               .InstancePerRequest();

            builder
              .RegisterType<TransactionRepository>()
              .As<ITransactionRepository>()
              .InstancePerRequest();

            base.Load(builder);
        }
    }
}

