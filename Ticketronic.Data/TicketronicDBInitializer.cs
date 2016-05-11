using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketronic.Domain;

namespace Ticketronic.Data
{
    public class TicketronicDBInitializer : DropCreateDatabaseAlways<TicketronicDBContext> // DropCreateDatabaseIfModelChanges<AccountDBContext>//CreateDatabaseIfNotExists, DropCreateDatabaseIfModelChanges, AlwaysRecreateDatabase 
    {
        protected override void Seed(TicketronicDBContext context)
        {
            var session1EventA  = new Session { Name = "Session 1", Duration = 4, Date = DateTime.Now, Starttime = new TimeSpan(8,00,00) };
          
            var sessionsEventA = new List<Session>
            {
                session1EventA
            };

            var session1EventB = new Session { Name = "Session 1", Duration = 3, Date = DateTime.Now, Starttime = new TimeSpan(8,00,00) };
            var session2EventB = new Session { Name = "Session 1", Duration = 8, Date = DateTime.Now.AddDays(1), Starttime = new TimeSpan(8, 00, 00) };
           
            var sessionsEventB = new List<Session>
            { 
                session1EventB,
                session2EventB 
            };

            var session1EventC = new Session { Name = "Session 1", Duration = 4, Date = DateTime.Now, Starttime = new TimeSpan(8,00,00) };
            var session2EventC =   new Session { Name = "Session 1", Duration = 4, Date = DateTime.Now.AddDays(1), Starttime = new TimeSpan(8,00,00) };
            var session3EventC =   new Session { Name = "Session 1", Duration = 4, Date = DateTime.Now.AddDays(2), Starttime = new TimeSpan(8,00,00) };
         

            var sessionsEventC = new List<Session>
            {
                session1EventC,
                session2EventC,
                session3EventC
            };

            var Events = new List<Event> {
                new Event { Name = "Event A", Duration = 1, Sessions = sessionsEventA } ,
                new Event { Name = "Event B", Duration = 2, Sessions = sessionsEventB } ,
                new Event { Name = "Event C", Duration = 3, Sessions = sessionsEventC } 
            };

            Events.ForEach(e => context.Events.Add(e));

            var ticket1 = new Ticket { Name = "Ticket Event A 1 day Pass", MinPurchase = 1, MaxPurchase = 1, Availability = 1, Price = 100.00f,
                                        Sessions = new List<Session> 
                                        { 
                                            session1EventA 
                                        } 
                                    };

            var ticket2 = new Ticket { Name = "Ticket Event B 1st day Pass", MinPurchase = 1, MaxPurchase = 1, Availability = 1, Price = 120.00f,
                                        Sessions = new List<Session> 
                                        { 
                                            session1EventB 
                                        } 
                                    };

            var ticket3 = new Ticket { Name = "Ticket Event B 2nd day Pass", MinPurchase = 1, MaxPurchase = 1, Availability = 1, Price = 120.00f,
                                        Sessions = new List<Session> 
                                        { 
                                            session2EventB 
                                        } 
                                    };

             var ticket4 = new Ticket { Name = "Ticket Event B 2 day Pass", MinPurchase = 1, MaxPurchase = 1, Availability = 2, Price = 200.00f,
                                        Sessions = new List<Session> 
                                        { 
                                            session1EventB ,
                                            session2EventB
                                        } 
                                    };

            var ticket5 = new Ticket { Name = "Ticket Event C 3 day Pass", MinPurchase = 1, MaxPurchase = 1, Availability = 3, Price = 300.00f,
                                        Sessions = new List<Session> 
                                        { 
                                            session1EventC,
                                            session2EventC,
                                            session3EventC,

                                        } 
                                    };


            var Tickets = new List<Ticket>{
                ticket1,ticket2, ticket3, ticket4, ticket5 
            };

            Tickets.ForEach(t => context.Tickets.Add(t));

            var Transactions = new List<Transaction>{
                new Transaction { Email = "C1@domain.com", ContactNo = "0123456789", FirstName = "C1", LastName = "C1",
                    TicketPurchases = new List<TicketPurchase> { 
                       new TicketPurchase { Quantity = 1 , Ticket = ticket1} 
                    }
                },
                new Transaction { Email = "C2@domain.com", ContactNo = "0123456789", FirstName = "C2", LastName = "C2",
                    TicketPurchases = new List<TicketPurchase> {
                        new TicketPurchase { Quantity = 1 , Ticket = ticket2} 
                    }
                },
                new Transaction { Email = "C3@domain.com", ContactNo = "0123456789", FirstName = "C3", LastName = "C3",
                    TicketPurchases = new List<TicketPurchase> {
                        new TicketPurchase { Quantity = 1 , Ticket = ticket3} ,
                        new TicketPurchase { Quantity = 1 , Ticket = ticket4} 
                    }
                 }
            };

            Transactions.ForEach(transaction => context.Transactions.Add(transaction));

        }
    }
}
