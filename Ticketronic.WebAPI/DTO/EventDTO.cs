using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ticketronic.Domain;

namespace Ticketronic.WebAPI
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public List<SessionDTO> Sessions { get; set; }
    }
}