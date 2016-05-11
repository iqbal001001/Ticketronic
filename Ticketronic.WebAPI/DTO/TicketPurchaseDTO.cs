using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticketronic.WebAPI
{
    public class TicketPurchaseDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int TransactionId { get; set; }
        //[ForeignKey("TransactionId")]
        //public Transaction Transaction { get; set; }

        public int TicketId { get; set; }
        //[ForeignKey("TicketId")]
        public TicketDTO Ticket { get; set; }

            
    }
}