using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketronic.Domain
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public float Price { get; set; }
        public int Availability { get; set; }
        public int MaxPurchase { get; set; }
        public int MinPurchase { get; set; }

        //public List<TicketPurchase> TicketPurchases { get; set; }

        public List<Session> Sessions { get; set; }
    }
}
