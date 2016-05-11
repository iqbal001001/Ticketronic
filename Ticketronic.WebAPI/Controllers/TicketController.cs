using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Data.Entity;
using Ticketronic.RepositoryInterface;
using Ticketronic.Domain;
using Ticketronic.WebAPI;
//using Ticketronic.WebAPI.Helper;
using System.Web.Http.Cors;
using System.Data.Entity.Validation;
using System.Web;
using System.Web.Http.Description;

namespace Ticketronic.WebAPI.Controllers
{
    //[Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TicketController : ApiController
    {
        private ITicketRepository _TicketRepo;
        private IEventRepository _EventRepo;
        private IUnitOfWork _uow;

        public TicketController(ITicketRepository TicketRepo, IEventRepository EventRepo, IUnitOfWork uow)
        {
            _TicketRepo = TicketRepo;
            _EventRepo = EventRepo;
            _uow = uow;
        }

        // GET: api/ticket
        [Route("api/ticket")]
        [ResponseType(typeof(List<TicketDTO>))]
        public IHttpActionResult Get()
        {
            try
            {
                var ticket = _TicketRepo.Get();
                var query = ticket
                    .ToList()
                    .Select(cl => cl.ToDTO()).ToList();

                return Ok(query);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/ticket/5
        [Route("api/ticket/{id}")]
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var query = _TicketRepo.Get();

                if ( query.Include(c => c.Sessions) != null ) query = query.Include(c => c.Sessions);
                
                var ticket = query.FirstOrDefault<Ticket>(c => c.Id == id);

                return ticket == null ? NotFound() : (IHttpActionResult)Ok(ticket.ToDTO());
               
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        // POST: api/ticket
        [Route("api/ticket")]
        [ResponseType(typeof(TicketDTO))] // Api Documentation
        public IHttpActionResult Post([FromBody]TicketDTO value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest();
                }

                string userName = "";
                if (HttpContext.Current != null && HttpContext.Current.User != null
                       && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                var releventSessions = GetReleventSessions(value);

                var ticket = value.ToDomain(releventSessions);

                _TicketRepo.Add(ticket);
                //                    client.CreateUser = userName;


                _uow.SaveChanges();

                if (ticket.Id > 0)
                {
                    return Created<TicketDTO>(Request.RequestUri + "/" + ticket.Id, ticket.ToDTO());
                }

                return BadRequest();

            }
            catch (DbEntityValidationException ex)
            {
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        // PUT: api/ticket/5
        [Route("api/ticket/{id}")]
        [ResponseType(typeof(TicketDTO))]
        public IHttpActionResult Put(int id, [FromBody]TicketDTO value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var originalTicket = _TicketRepo.Get()
                //need to load Tickets for CRUD on ManytoMant
                .Include(s => s.Sessions)
                   .FirstOrDefault<Ticket>(c => c.Id == id);

            if (originalTicket == null)
            {
                return NotFound();
            }

            string userName = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                   && HttpContext.Current.User.Identity.Name != null)
            {
                userName = HttpContext.Current.User.Identity.Name;
            }

            var releventSessions = GetReleventSessions(value);

            //List<Session> Sessions = _EventRepo.Get().Include(s => s.Sessions).Where(c => AllEvents.Contains(c.Id)).SelectMany(x => x.Sessions).Include(ee => ee.Event).ToList();

            var ticket = value.ToDomain(releventSessions, originalTicket);
            //       client.ChangeUser = userName;


            _TicketRepo.Update(ticket);
            try
            {
                _uow.SaveChanges();

                return Created<TicketDTO>(Request.RequestUri + "/" + ticket.Id, ticket.ToDTO());
            }
            catch (DbEntityValidationException ex)
            {
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private List<Session> GetReleventSessions(TicketDTO value)
        {
            var AllEvents = value.Sessions.Select(s => s.EventId); // domain event from DTO event

            var AllSessions = _EventRepo.Get().Include(s => s.Sessions).Where(c => AllEvents.Contains(c.Id)).SelectMany(x => x.Sessions); // All sessions in all events

            var releventSessionId = value.Sessions.Select(vs => vs.Id).ToList();

            var releventSessions = AllSessions.Where(s => releventSessionId.Contains(s.Id)).Include(t => t.Tickets).ToList(); // need to load Tickets for CRUD on ManytoMant
            return releventSessions;
        }

        // DELETE: api/ticket/5
        [Route("api/ticket/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (_TicketRepo.Get().FirstOrDefault<Ticket>(c => c.Id == id) == null)
                {
                    return NotFound();
                }

                _TicketRepo.Delete(id);

                _uow.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbEntityValidationException ex)
            {
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
    }
}
