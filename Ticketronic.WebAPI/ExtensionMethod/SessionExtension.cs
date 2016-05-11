using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ticketronic.Domain;

namespace Ticketronic.WebAPI
{
    public static class SessionExtension
    {
        public static SessionDTO ToDTO(this Session session)
        {
            return new SessionDTO
            {
                Id = session.Id,
                Name = session.Name != null ? session.Name.Trim() : null,
                Date = session.Date,
                Starttime = session.Starttime,
                Duration = session.Duration,
                EventId = session.EventId

            };
        }

        public static Session ToDomain(this SessionDTO session, Session originalSession = null)
        {
            if (originalSession != null && originalSession.Id == session.Id)
            {
                originalSession.Name = session.Name;
                originalSession.Date = session.Date;
                originalSession.Starttime = session.Starttime;
                originalSession.Duration = session.Duration;
                

                return originalSession;
            }
            return new Session
            {
                Id = session.Id,
                Name = session.Name,
                Date = session.Date,
                Starttime = session.Starttime,
                Duration = session.Duration

            };
        }

    }
}