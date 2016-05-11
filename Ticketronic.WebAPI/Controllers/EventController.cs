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
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EventController : ApiController
    {
        private IEventRepository _EventRepo;
        private IUnitOfWork _uow;

        public EventController(IEventRepository EventRepo, IUnitOfWork uow)
        {
            _EventRepo = EventRepo;
            _uow = uow;
        }

        // GET: api/event
        [Route("api/event", Name = "EventList")]
        [ResponseType(typeof(List<EventDTO>))]
        public IHttpActionResult Get()
        {
            try
            {
                var ev = _EventRepo.Get();
                var query = ev
                    .ToList()
                    .Select(cl => cl.ToDTO()).ToList();

                return Ok(query);

            }
             catch (Exception ex)
             {
                 return InternalServerError(ex);
             }
        }

        // GET: api/event/5
        [Route("api/event/{id}")]
        [ResponseType(typeof(EventDTO))]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var query = _EventRepo.Get();

                // iif condition to check if property is null For Unit Test Moq
                if (query.Include(c => c.Sessions) != null) query = query.Include(c => c.Sessions);

                var ev = query.FirstOrDefault<Event>(c => c.Id == id);

                return ev == null ?  NotFound() : (IHttpActionResult)Ok(ev.ToDTO());
               
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }
           
        }

        // POST: api/event
        [Route("api/event")]
        [ResponseType(typeof(EventDTO))] // Api Documentation
        public IHttpActionResult Post([FromBody]EventDTO value)
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

                    var ev = value.ToDomain();

                    _EventRepo.Add(ev);
//                    client.CreateUser = userName;


                    _uow.SaveChanges();

                    if (ev.Id > 0)
                    {
                        return Created<EventDTO>(Request.RequestUri + "/" + ev.Id, ev.ToDTO());
                    }

                    return BadRequest();
               
            }
            catch (DbEntityValidationException ex)
            {
               return InternalServerError();
            }
            catch(Exception ex)
            {
               return InternalServerError();
            }
        }

        // PUT: api/event/5
        [Route("api/event/{id}")]
        [ResponseType(typeof(EventDTO))]
        public IHttpActionResult Put(int id, [FromBody]EventDTO value)
        {  
                if (value == null)
                {
                    return BadRequest();
                }
             var originalClient = _EventRepo.Get()
                    //.Include(c => c.Sessions)
                    .FirstOrDefault<Event>(c => c.Id == id);

             if (originalClient == null)
                {
                    return NotFound();
                }

             string userName = "";
             if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
             {
                 userName = HttpContext.Current.User.Identity.Name;
             }


                var client = value.ToDomain(originalClient);
         //       client.ChangeUser = userName;


                _EventRepo.Update(client);
 try
            {
                _uow.SaveChanges();

                return Created<EventDTO>(Request.RequestUri + "/" + client.Id, client.ToDTO());
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

        // DELETE: api/event/5
        [Route("api/event/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (_EventRepo.Get().First<Event>(c => c.Id == id) == null)
                {
                    return NotFound();
                }

                _EventRepo.Delete(id);

                _uow.SaveChanges();

                return  StatusCode(HttpStatusCode.NoContent);
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
