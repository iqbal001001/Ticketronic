using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ticketronic.Domain;

namespace Ticketronic.WebAPI
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Availability { get; set; }
        public int MaxPurchase { get; set; }
        public int MinPurchase { get; set; }

        public List<SessionDTO> Sessions { get; set; }
    }
}