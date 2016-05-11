using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;
using System.Web;
using Ticketronic.Domain;

namespace Ticketronic.WebAPI
{
    public static class EventExtension
    {
        public static EventDTO ToDTO(this Event ev)
        {
            return new EventDTO
            {
                Id = ev.Id,
                Name = ev.Name != null ? ev.Name.Trim() : null,
                Duration = ev.Duration,
                Sessions = ev.Sessions != null ? ev.Sessions.Select(s => s.ToDTO()).ToList() : new List<SessionDTO>(),
                
            };
        }

        public static Event ToDomain(this EventDTO ev, Event originalEv = null)
        {
            if (originalEv != null && originalEv.Id == ev.Id)
            {
                originalEv.Name = ev.Name;
                originalEv.Duration = ev.Duration;
                originalEv.Sessions = ev.Sessions != null ?
                    ev.Sessions.Select(s => s.ToDomain(
                            originalEv.Sessions != null ? originalEv.Sessions.FirstOrDefault(os => os.Id == s.Id) : null
                            )).ToList() : new List<Session>();


                return originalEv;
            }
            return new Event
            {
                Id = ev.Id,
                Name = ev.Name,
                Duration = ev.Duration,
                Sessions = ev.Sessions != null ? ev.Sessions.Select(s => s.ToDomain()).ToList() : new List<Session>()
          
                
            };
        }

      
    }
}