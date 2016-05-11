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
    public class TransactionController : ApiController
    {
        private ITransactionRepository _TransactionRepo;
        private IUnitOfWork _uow;

        public TransactionController(ITransactionRepository TransactionRepo, IUnitOfWork uow)
        {
            _TransactionRepo = TransactionRepo;
            _uow = uow;
        }

        // GET: api/transaction
        [Route("api/transaction")]
        [ResponseType(typeof(List<TransactionDTO>))]
        public IHttpActionResult Get()
        {
            try
            {
                var transaction = _TransactionRepo.Get();
                var query = transaction
                    .ToList()
                    .Select(t => t.ToDTO()).ToList();

                return Ok(query);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/transaction/5
        [Route("api/transaction/{id}")]
        [ResponseType(typeof(TransactionDTO))]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var query = _TransactionRepo.Get(); //.Include(c => c.TicketPurchases);

                if (query.Include(c => c.TicketPurchases) != null) query = query.Include(c => c.TicketPurchases);

                var transaction = query.FirstOrDefault<Transaction>(c => c.Id == id);

                return transaction == null ? NotFound() : (IHttpActionResult)Ok(transaction.ToDTO());
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        // PUT: api/transaction
        [Route("api/transaction")]
        [ResponseType(typeof(TransactionDTO))]
        public IHttpActionResult Post([FromBody]TransactionDTO value)
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

                var transaction = value.ToDomain();

                _TransactionRepo.Add(transaction);
                //                    client.CreateUser = userName;


                _uow.SaveChanges();

                if (transaction.Id > 0)
                {
                    return Created<TransactionDTO>(Request.RequestUri + "/" + transaction.Id, transaction.ToDTO());
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

        // PUT: api/transaction/5
        [Route("api/transaction/{id}")]
        [ResponseType(typeof(TransactionDTO))]
        public IHttpActionResult Put(int id, [FromBody]TransactionDTO value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var originalTransactionQuery = _TransactionRepo.Get();
            if (originalTransactionQuery.Include(c => c.TicketPurchases) != null)
                originalTransactionQuery = originalTransactionQuery.Include(c => c.TicketPurchases);

            var originalTransaction = originalTransactionQuery.FirstOrDefault<Transaction>(c => c.Id == id);

            if (originalTransaction == null)
            {
                return NotFound();
            }

            string userName = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                   && HttpContext.Current.User.Identity.Name != null)
            {
                userName = HttpContext.Current.User.Identity.Name;
            }


            var transaction = value.ToDomain(originalTransaction);
            //       client.ChangeUser = userName;


            _TransactionRepo.Update(transaction);
            try
            {
                _uow.SaveChanges();

                return Created<TransactionDTO>(Request.RequestUri + "/" + transaction.Id, transaction.ToDTO());
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

        // DELETE: api/transaction/5
        [Route("api/transaction/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (_TransactionRepo.Get().First<Transaction>(c => c.Id == id) == null)
                {
                    return NotFound();
                }

                _TransactionRepo.Delete(id);

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
