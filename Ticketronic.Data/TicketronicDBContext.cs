using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketronic.Domain;

namespace Ticketronic.Data
{
    public class TicketronicDBContext : DbContext
    {
        public TicketronicDBContext()
            : base("Ticketronic")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TicketPurchase> TicketPurchases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Event>()
                         .HasMany(e => e.Sessions)
                         .WithRequired(e => e.Event)
                         //.HasForeignKey(c => c.)
                         .WillCascadeOnDelete(true);

         //   modelBuilder.Entity<TicketPurchase>()
         //.HasKey(c => new { c.Id, c.TransactionId });

            //modelBuilder.Entity<Ticket>()
            //            .HasMany(e=>e.TicketPurchases)
            //            .WithRequired(e=>e.Ticket)
            //           .WillCascadeOnDelete(false);


            //modelBuilder.Entity<Transaction>()
            //           .HasMany(e => e.TicketPurchases)
            //           .WithRequired(e => e.Transaction)
            //    .HasForeignKey(c => c.TransactionId)
            //           .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Ticket>().HasMany(e => e.Sessions).WithMany).WillCascadeOnDelete(true);
                         //   .HasRequired(e => e.Sessions).
                         //.WithMany(e => e);
                         
                      //   .WillCascadeOnDelete(true);
        }

    }
}
