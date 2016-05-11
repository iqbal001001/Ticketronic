using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ticketronic.Domain;

namespace Ticketronic.WebAPI
{
    public static class TicketPurchaseExtension
    {
        public static TicketPurchaseDTO ToDTO(this TicketPurchase ticket)
        {
            return new TicketPurchaseDTO
            {
                Id = ticket.Id,
                Quantity = ticket.Quantity,
                TicketId = ticket.TicketId
                 

                //Sessions = ticket.Sessions != null ? ticket.Sessions.Select(s => s.ToDTO()).ToList() : new List<SessionDTO>(),
                
            };
        }

        public static TicketPurchase ToDomain(this TicketPurchaseDTO ticket, TicketPurchase originalTicket = null)
        {
            if (originalTicket != null && originalTicket.Id == ticket.Id)
            {
                originalTicket.Quantity = ticket.Quantity;

                originalTicket.TicketId = ticket.TicketId;

                return originalTicket;
            }
            return new TicketPurchase
            {
                Id = ticket.Id,
                Quantity = ticket.Quantity,
               TicketId = ticket.TicketId
             

            };
        }
    }
}