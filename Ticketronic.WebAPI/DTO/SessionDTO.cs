using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ticketronic.Domain;

namespace Ticketronic.WebAPI
{
    public class SessionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Starttime { get; set; }
        public float Duration { get; set; }
        public int EventId { get; set; }
        //public EventDTO Event { get; set; }
    }
}