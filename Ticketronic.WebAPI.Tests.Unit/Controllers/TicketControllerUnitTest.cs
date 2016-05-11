//using System;
//using System.Collections;
//using System.Collections.Specialized;
//using System.Data.Entity;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.Mvc;
//using System.Net.Http;
//using System.Web.Http.Results;
//using System.Web.Routing;
//using System.IO;
//using System.Web.Hosting;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using MvcContrib;
//using MvcContrib.TestHelper;
//using System.Data.Entity.Infrastructure;
//using System.Threading.Tasks;
//using Ticketronic.Domain;
//using Ticketronic.RepositoryInterface;
//using Ticketronic.WebAPI.Controllers;
//using System.Web.Http;

//namespace Ticketronic.WebAPI.Tests.Unit.Controllers
//{
//    [TestClass]
//    public class TicketConttrollerUnitTest
//    {
//        private IQueryable<Ticket> _data;
//        private Mock<DbSet<Ticket>> _mockSet;
//        private Mock<IUnitOfWork> _mockUOW;
//        private Mock<ITicketRepository> _mocTR;

//        private Mock<IUnitOfWork> _mockUW;
//        private Mock<HttpContextBase> _mockHttpContext;
//        private Mock<HttpRequestBase> _mockRequest;
//        private Mock<HttpResponseBase> _mockResponse;
//        private NameValueCollection _FormKeys;


//        public TicketConttrollerUnitTest()
//        {
//            _data = new List<Ticket> 
//            { 
//                new Ticket {Id = 1,  Name = "Ticket A", Availability = 2, Price = 100, MaxPurchase = 3, MinPurchase = 1}, 
//                new Ticket {Id = 2,  Name = "Ticket B", Availability = 2, Price = 100, MaxPurchase = 3, MinPurchase = 1},
//                new Ticket {Id = 3,  Name = "Ticket C", Availability = 2, Price = 100, MaxPurchase = 3, MinPurchase = 1},
//            }.AsQueryable();


//            _mockSet = new Mock<DbSet<Ticket>>();
//            _mockSet.As<IQueryable<Ticket>>().Setup(m => m.Provider).Returns(_data.Provider);
//            _mockSet.As<IQueryable<Ticket>>().Setup(m => m.Expression).Returns(_data.Expression);
//            _mockSet.As<IQueryable<Ticket>>().Setup(m => m.ElementType).Returns(_data.ElementType);
//            _mockSet.As<IQueryable<Ticket>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());

//            _mockUOW = new Mock<IUnitOfWork>();
//            _mockUOW.Setup(t => t.SaveChanges()).Verifiable();

//            _mocTR = new Mock<ITicketRepository>();
//            _mocTR.Setup(t => t.Get()).Returns(_mockSet.Object);
//            _mocTR.Setup(t => t.Get(x => x.Id == It.IsAny<int>())).Returns(_mockSet.Object);
//            _mocTR.Setup(t => t.Add(It.IsAny<Ticket>())).Callback((Ticket ticket) =>
//            {
//                var newListEmployee = new List<Ticket> { ticket };
//                _data = _data.Concat(newListEmployee);
//            }).Verifiable();
//            _mocTR.Setup(t => t.Delete(It.IsAny<Ticket>())).Callback((Ticket ticket) =>
//            {
//                //var newListTicket = new List<Ticket> { ticket };
//                var list = new List<Ticket>();
//                list = _data.ToList();
//                list.Remove(ticket);
//                _data = list.AsQueryable();
//            }).Verifiable();
//            //_mocCR.Verify(mr => mr.Update(It.IsAny<Employee>()), Times.Once());
//            //_mocCR.Setup(t => t.Delete(It.IsAny<int>()));
//            //_mocCR.Setup(t => t.Delete(It.IsAny<Employee>()));
//            _mocTR.Setup(t => t.Update(It.IsAny<Ticket>()));

//            _mockUW = new Mock<IUnitOfWork>();
//            _mockUW.Setup(t => t.SaveChanges());

//            //reference : http://justinchmura.com/2014/06/26/mock-httpcontext/
//            _mockHttpContext = new Mock<HttpContextBase>();
//            _mockRequest = new Mock<HttpRequestBase>();
//            _mockResponse = new Mock<HttpResponseBase>();
//            _mockResponse.SetupGet(req => req.Headers).Returns(new NameValueCollection());
//            _FormKeys = new NameValueCollection();
//            _mockHttpContext.Setup(ctxt => ctxt.Request).Returns(_mockRequest.Object);
//            _mockHttpContext.Setup(ctxt => ctxt.Response).Returns(_mockResponse.Object);
//            // _mockHttpContext.Setup(ctxt => ctxt.ApplicationInstance).Returns(_mockHttpContext.Object);
//            //_mockHttpContext.Setup(ctxt => ctxt.current).Returns(_mockResponse.Object);
//            _mockRequest.Setup(r => r.Form).Returns(_FormKeys);


//        }

//        [TestInitialize]
//        public void TestSetup()
//        {
//            HttpContext.Current = null;
//        }


//        [TestMethod]
//        public void Get_ShouldReturnAllTickets()
//        {

//            var controller = new TicketController(_mocTR.Object, _mockUOW.Object);

//            controller.Request = new HttpRequestMessage();
//            var routeData = new RouteData();

//            controller.Configuration = new HttpConfiguration();

//            var result = controller.Get() as IHttpActionResult;
//            Assert.IsTrue(result is OkNegotiatedContentResult<List<TicketDTO>>);
//            var okResult = result as OkNegotiatedContentResult<List<TicketDTO>>;
//            Assert.AreEqual(_data.Count(), okResult.Content.Count());

//        }

//        [TestMethod]
//        public void Get_byID_ShouldReturnOneTickets()
//        {
//            var controller = new TicketController(_mocTR.Object, _mockUOW.Object);

//            var result = controller.Get(1) as IHttpActionResult;
//            Assert.IsTrue(result is OkNegotiatedContentResult<TicketDTO>);

//            var okResult = result as OkNegotiatedContentResult<TicketDTO>;
//            Assert.AreEqual(1, okResult.Content.Id);
//        }

//        [TestMethod]
//        public void Post_ShouldInsertTicket()
//        {

//            var controller = new TicketController(_mocTR.Object, _mockUOW.Object);
//            var newTicket = new TicketDTO { Id = 4, Name = "Ticket D", Availability = 2, Price = 100, MaxPurchase = 3, MinPurchase = 1 };
//            controller.Request = new HttpRequestMessage();
//            controller.Request.RequestUri = new Uri("http://test");

//            var result = controller.Post(newTicket) as IHttpActionResult;

//            _mocTR.Verify(
//                cr => cr.Add(It.Is<Ticket>(c => c.Id == 4)),
//                    Times.Once);
//            _mockUOW.Verify(
//                uow => uow.SaveChanges(),
//                Times.Once);

//            Assert.IsTrue(result is CreatedNegotiatedContentResult<TicketDTO>);

//            var createdResult = result as CreatedNegotiatedContentResult<TicketDTO>;
//            Assert.AreEqual(newTicket.Id, createdResult.Content.Id);
//            Assert.AreEqual(controller.Request.RequestUri.ToString() + "/" + newTicket.Id, createdResult.Location.ToString());

//        }

//        [TestMethod]
//        public void Put_ShouldUpdateTicket()
//        {

//            var controller = new TicketController(_mocTR.Object, _mockUOW.Object);
//            var ticket = new TicketDTO { Id = 1, Name = "Ticket AA", Availability = 2, Price = 100, MaxPurchase = 3, MinPurchase = 1 };
//            controller.Request = new HttpRequestMessage();
//            controller.Request.RequestUri = new Uri("http://test");

//            var result = controller.Put(1, ticket) as IHttpActionResult;

//            _mocTR.Verify(
//                cr => cr.Update(It.Is<Ticket>(c => c.Id == 1)),
//                    Times.Once);
//            _mockUOW.Verify(
//                uow => uow.SaveChanges(),
//                Times.Once);

//            Assert.IsTrue(result is CreatedNegotiatedContentResult<TicketDTO>);

//            var createdResult = result as CreatedNegotiatedContentResult<TicketDTO>;
//            Assert.AreEqual(ticket.Id, createdResult.Content.Id);
//            Assert.AreEqual(ticket.Name, createdResult.Content.Name);
//            Assert.AreEqual(controller.Request.RequestUri.ToString() + "/" + ticket.Id, createdResult.Location.ToString());

//        }

//        [TestMethod]
//        public void Delete_ShouldRemoveTicket()
//        {

//            var controller = new TicketController(_mocTR.Object, _mockUOW.Object);
//            var ticket = _data.First<Ticket>(c => c.Id == 1);

//            var result = controller.Delete(1) as IHttpActionResult;

//            _mocTR.Verify(
//                cr => cr.Delete(It.Is<int>(c => c.Equals(1))),
//                    Times.Once);
//            _mockUOW.Verify(
//                uow => uow.SaveChanges(),
//                Times.Once);

//            Assert.IsTrue(result is StatusCodeResult);

//            var statusResult = result as StatusCodeResult;
//            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, statusResult.StatusCode);

//        }
//    }
//}
