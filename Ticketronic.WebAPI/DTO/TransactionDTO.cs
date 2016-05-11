using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ticketronic.Domain;

namespace Ticketronic.WebAPI
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        //public byte[] TimeStamp { get; }

        public List<TicketPurchaseDTO> Tickets { get; set; }
    }
}