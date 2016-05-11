using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ticketronic.Domain;

namespace Ticketronic.WebAPI
{
    public static class TicketExtension
    {
        public static TicketDTO ToDTO(this Ticket ticket)
        {
            return new TicketDTO
            {
                Id = ticket.Id,
                Name = ticket.Name != null ? ticket.Name.Trim() : null,
                Price = ticket.Price,
                Availability = ticket.Availability,
                MaxPurchase = ticket.MaxPurchase,
                MinPurchase = ticket.MinPurchase,
                Sessions = ticket.Sessions != null ? ticket.Sessions.Select(s => s.ToDTO()).ToList() : new List<SessionDTO>(),
                
            };
        }

        public static Ticket ToDomain(this TicketDTO ticket, List<Session> originalSessions, Ticket originalTicket = null)
        {
            if (originalTicket != null && originalTicket.Id == ticket.Id)
            {
                originalTicket.Name = ticket.Name;
                originalTicket.Price = ticket.Price;
                originalTicket.Availability = ticket.Availability;
                originalTicket.MaxPurchase = ticket.MaxPurchase;
                originalTicket.MinPurchase = ticket.MinPurchase;
                originalTicket.Sessions = ticket.Sessions != null ?
                    ticket.Sessions.Select(s => s.ToDomain(originalSessions.First(os => os.Id == s.Id))).ToList() : new List<Session>(); // Should always find a session

                return originalTicket;
            }
            return new Ticket
            {
                Id = ticket.Id,
                Name = ticket.Name,
                Price = ticket.Price,
                Availability = ticket.Availability,
                MaxPurchase = ticket.MaxPurchase,
                MinPurchase = ticket.MinPurchase,
                Sessions = ticket.Sessions != null ? 
                    ticket.Sessions.Select(s => s.ToDomain(originalSessions.First(os=>os.Id == s.Id))).ToList() : new List<Session>() // should always find a session 

            };
        }
    }
}