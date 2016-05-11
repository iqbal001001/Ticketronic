using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketronic.Domain
{
    public class TicketPurchase
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        //public int TransactionId { get; set; }
        //[ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }

        public int TicketId { get; set; }
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }

            
    }
}
