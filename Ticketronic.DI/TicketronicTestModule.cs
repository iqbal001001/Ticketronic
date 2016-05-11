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
    public class TicketronicTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterModule<TicketronicModule>();

            builder
                .RegisterType<TicketronicDBTestContextFactory>()
                .As<IDbContextFactory>()
                .InstancePerRequest();

            base.Load(builder);
        }
    }
}

