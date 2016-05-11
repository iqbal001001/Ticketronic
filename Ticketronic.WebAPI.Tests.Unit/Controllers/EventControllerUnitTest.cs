using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http.Results;
using System.Web.Routing;
using System.IO;
using System.Web.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcContrib;
using MvcContrib.TestHelper;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Ticketronic.Domain;
using Ticketronic.RepositoryInterface;
using Ticketronic.WebAPI.Controllers;
using System.Web.Http;

namespace Ticketronic.WebAPI.Tests.Unit.Controllers
{
    [TestClass]
    public class EventConttrollerUnitTest
    {
        private IQueryable<Event> _data;
        private Mock<DbSet<Event>> _mockSet;
        private Mock<IUnitOfWork> _mockUOW;
        private Mock<IEventRepository> _mocCR;
        private Mock<IUnitOfWork> _mockUW;
        private Mock<HttpContextBase> _mockHttpContext;
        private Mock<HttpRequestBase> _mockRequest;
        private Mock<HttpResponseBase> _mockResponse;
        private NameValueCollection _FormKeys;


        public EventConttrollerUnitTest()
        {
            _data = new List<Event> 
            { 
                new Event {Id = 1,  Name = "Event A", Duration = 3, Sessions = new List<Session> {}}, 
                new Event {Id = 2,  Name = "Event B", Duration = 4, Sessions = new List<Session> {}},
                new Event {Id = 3,  Name = "Event C", Duration = 5, Sessions = new List<Session> {}},
            }.AsQueryable();


            _mockSet = new Mock<DbSet<Event>>();
            _mockSet.As<IQueryable<Event>>().Setup(m => m.Provider).Returns(_data.Provider);
            _mockSet.As<IQueryable<Event>>().Setup(m => m.Expression).Returns(_data.Expression);
            _mockSet.As<IQueryable<Event>>().Setup(m => m.ElementType).Returns(_data.ElementType);
            _mockSet.As<IQueryable<Event>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());

            _mockUOW = new Mock<IUnitOfWork>();
            _mockUOW.Setup(t => t.SaveChanges()).Verifiable();

            _mocCR = new Mock<IEventRepository>();
            _mocCR.Setup(t => t.Get()).Returns(_mockSet.Object);
            _mocCR.Setup(t => t.Get(x => x.Id == It.IsAny<int>())).Returns(_mockSet.Object);
            _mocCR.Setup(t => t.Add(It.IsAny<Event>())).Callback((Event ticket) =>
            {
                var newListEmployee = new List<Event> { ticket };
                _data = _data.Concat(newListEmployee);
            }).Verifiable();
            _mocCR.Setup(t => t.Delete(It.IsAny<Event>())).Callback((Event ticket) =>
            {
                //var newListEvent = new List<Event> { ticket };
                var list = new List<Event>();
                list = _data.ToList();
                list.Remove(ticket);
                _data = list.AsQueryable();
            }).Verifiable();

            _mocCR.Setup(t => t.Update(It.IsAny<Event>()));

            _mockUW = new Mock<IUnitOfWork>();
            _mockUW.Setup(t => t.SaveChanges());

            //reference : http://justinchmura.com/2014/06/26/mock-httpcontext/
            _mockHttpContext = new Mock<HttpContextBase>();
            _mockRequest = new Mock<HttpRequestBase>();
            _mockResponse = new Mock<HttpResponseBase>();
            _mockResponse.SetupGet(req => req.Headers).Returns(new NameValueCollection());
            _FormKeys = new NameValueCollection();
            _mockHttpContext.Setup(ctxt => ctxt.Request).Returns(_mockRequest.Object);
            _mockHttpContext.Setup(ctxt => ctxt.Response).Returns(_mockResponse.Object);
            _mockRequest.Setup(r => r.Form).Returns(_FormKeys);


        }

        [TestInitialize]
        public void TestSetup()
        {
            HttpContext.Current = null;
        }


        [TestMethod]
        public void Get_ShouldReturnAllEvents()
        {

            var controller = new EventController(_mocCR.Object, _mockUOW.Object);

            controller.Request = new HttpRequestMessage();
            var routeData = new RouteData();

            controller.Configuration = new HttpConfiguration();

            var result = controller.Get() as IHttpActionResult;
            Assert.IsTrue(result is OkNegotiatedContentResult<List<EventDTO>>);
            var okResult = result as OkNegotiatedContentResult<List<EventDTO>>;
            Assert.AreEqual(_data.Count(), okResult.Content.Count());

        }

        [TestMethod]
        public void Get_byID_ShouldReturnOneEvents()
        {
            var controller = new EventController(_mocCR.Object, _mockUOW.Object);

            var result = controller.Get(1) as IHttpActionResult;
            Assert.IsTrue(result is OkNegotiatedContentResult<EventDTO>);

            var okResult = result as OkNegotiatedContentResult<EventDTO>;
            Assert.AreEqual(1, okResult.Content.Id);
        }

        [TestMethod]
        public void Post_ShouldInsertEvent()
        {

            var controller = new EventController(_mocCR.Object, _mockUOW.Object);
            var newEvent = new EventDTO { Id = 4, Name = "Event D", Duration = 3, Sessions = null };
            controller.Request = new HttpRequestMessage();
            controller.Request.RequestUri = new Uri("http://test");

            var result = controller.Post(newEvent) as IHttpActionResult;

            _mocCR.Verify(
                cr => cr.Add(It.Is<Event>(c => c.Id == 4)),
                    Times.Once);
            _mockUOW.Verify(
                uow => uow.SaveChanges(),
                Times.Once);

            Assert.IsTrue(result is CreatedNegotiatedContentResult<EventDTO>);

            var createdResult = result as CreatedNegotiatedContentResult<EventDTO>;
            Assert.AreEqual(newEvent.Id, createdResult.Content.Id);
            Assert.AreEqual(controller.Request.RequestUri.ToString() + "/" + newEvent.Id, createdResult.Location.ToString());

        }

        [TestMethod]
        public void Put_ShouldUpdateEvent()
        {

            var controller = new EventController(_mocCR.Object, _mockUOW.Object);
            var ticket = new EventDTO { Id = 1, Name = "Event AA",Duration = 3, Sessions = null };
            controller.Request = new HttpRequestMessage();
            controller.Request.RequestUri = new Uri("http://test");

            var result = controller.Put(1, ticket) as IHttpActionResult;

            _mocCR.Verify(
                cr => cr.Update(It.Is<Event>(c => c.Id == 1)),
                    Times.Once);
            _mockUOW.Verify(
                uow => uow.SaveChanges(),
                Times.Once);

            Assert.IsTrue(result is CreatedNegotiatedContentResult<EventDTO>);

            var createdResult = result as CreatedNegotiatedContentResult<EventDTO>;
            Assert.AreEqual(ticket.Id, createdResult.Content.Id);
            Assert.AreEqual(ticket.Name, createdResult.Content.Name);
            Assert.AreEqual(controller.Request.RequestUri.ToString() + "/" + ticket.Id, createdResult.Location.ToString());

        }

        [TestMethod]
        public void Delete_ShouldRemoveEvent()
        {

            var controller = new EventController(_mocCR.Object, _mockUOW.Object);
            var ticket = _data.First<Event>(c => c.Id == 1);

            var result = controller.Delete(1) as IHttpActionResult;

            _mocCR.Verify(
                cr => cr.Delete(It.Is<int>(c => c.Equals(1))),
                    Times.Once);
            _mockUOW.Verify(
                uow => uow.SaveChanges(),
                Times.Once);

            Assert.IsTrue(result is StatusCodeResult);

            var statusResult = result as StatusCodeResult;
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, statusResult.StatusCode);

        }
    }
}
