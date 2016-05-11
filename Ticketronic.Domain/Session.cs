using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketronic.Domain
{
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EventId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime Date { get; set;}
        public TimeSpan Starttime { get; set; }
        public float Duration { get; set; }

        public Event Event { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
