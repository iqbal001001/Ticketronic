using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.SessionState;
using System.Security.Principal;
using Owin;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Testing;
using System.IO;
using System.Threading;
using Ticketronic.Domain;
using Ticketronic.RepositoryInterface;
using Ticketronic.WebAPI;

//reference : https://blogs.msdn.microsoft.com/webdev/2013/11/26/unit-testing-owin-applications-using-testserver/
//reference : http://www.strathweb.com/2012/06/asp-net-web-api-integration-testing-with-in-memory-hosting/
//reference : http://dotnetliberty.com/index.php/2015/12/17/asp-net-5-web-api-integration-testing/

namespace Ticketronic.WebAPI.Tests.Integration
{
    [TestClass]
    public class TicketIntTest
    {
        //private IDisposable webApp;
        //private string baseAddress = "http://localhost/";
        

        [TestMethod]
        public void Get_ShouldReturnAllTickets()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                var client = server.HttpClient;
                HttpResponseMessage response = client.GetAsync("api/Ticket").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<TicketDTO>>().Result;
                Assert.AreEqual(5, result.Count());
            }
        }

        [Ignore]
        [TestMethod]
        public void Get_ShouldReturnAllTickets_page_pageSize()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.CreateRequest("api/Ticket?page=1&pageSize=2")
                                                        .AddHeader("header1", "headervalue1")
                                                        .GetAsync().Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<TicketDTO>>().Result;
                Assert.AreEqual(3, result.Count());

                //HttpResponseMessage response2 = server.HttpClient.GetAsync("api/Ticket?page=2&pageSize=2").Result;

                //Assert.IsTrue(response2.IsSuccessStatusCode);
                //Assert.AreEqual(System.Net.HttpStatusCode.OK, response2.StatusCode);

                //var result2 = response2.Content.ReadAsAsync<List<TicketDTO>>().Result;
                //Assert.AreEqual(1, result2.Count());

                // TODO : HTTPContext test for Pagination Object
            }
        }

        [TestMethod]
        public void Get_Single_ShouldReturnTickets()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Ticket/1").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<TicketDTO>().Result;
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(1, result.Sessions.Count);
                Assert.AreEqual(1, result.Sessions[0].Id);
            }
        }

        [TestMethod]
        public void Post_ShouldInsertTicket()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                // get the session from Event
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Event/1").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<EventDTO>().Result;
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(1, result.Sessions.Count);
                Assert.AreEqual(1, result.Sessions[0].Id);

                var session = result.Sessions[0];

                // Testing the ticket
                var sessions = new List<SessionDTO> { session };

                var data = new TicketDTO { Name = "Ticket D", Availability = 2, Sessions = sessions };
                HttpResponseMessage response2 = server.HttpClient.PostAsJsonAsync("api/Ticket", data).Result;

                Assert.IsTrue(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.Created, response2.StatusCode);

                var result2 = response2.Content.ReadAsAsync<TicketDTO>().Result;
                Assert.AreEqual(data.Name, result2.Name);
                Assert.AreEqual(data.Sessions.Count, result2.Sessions.Count);
                Assert.AreEqual(data.Sessions[0].Name, result2.Sessions[0].Name);
            }
        }

        [TestMethod]
        public void Put_ShouldUpdateTicket_With_AddSession()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                // get the session from an Event
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Event/1").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<EventDTO>().Result;
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(1, result.Sessions.Count);
                Assert.AreEqual(1, result.Sessions[0].Id);

                // get Original ticket
                HttpResponseMessage response1 = server.HttpClient.GetAsync("api/Ticket/3").Result;

                Assert.IsTrue(response1.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response1.StatusCode);

                var data = response1.Content.ReadAsAsync<TicketDTO>().Result;
                data.Name = "Ticket CC";
                data.Sessions.Add(result.Sessions[0]); // associate session to Ticket

                // Update Ticket
                HttpResponseMessage response2 = server.HttpClient.PutAsJsonAsync("api/Ticket/3", data).Result;

                Assert.IsTrue(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.Created, response2.StatusCode);

                var result2 = response2.Content.ReadAsAsync<TicketDTO>().Result;

                Assert.AreEqual(data.Name, result2.Name);

                // get modified ticket
                HttpResponseMessage response3 = server.HttpClient.GetAsync("api/Ticket/3").Result;

                Assert.IsTrue(response3.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response3.StatusCode);

                var data2 = response3.Content.ReadAsAsync<TicketDTO>().Result;

                Assert.AreEqual(data2.Name, data.Name);
                Assert.AreEqual(data.Sessions.Count(), result2.Sessions.Count());
            }

        }

        [TestMethod]
        public void Put_ShouldUpdateTicket_With_RemoveSession()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                // get orginal ticket
                HttpResponseMessage response1 = server.HttpClient.GetAsync("api/Ticket/5").Result;

                Assert.IsTrue(response1.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response1.StatusCode);

                var data = response1.Content.ReadAsAsync<TicketDTO>().Result;
                data.Name = "Ticket 5a";
                data.Sessions.RemoveAt(0); // remove session from Ticket

                //save modified ticket
                HttpResponseMessage response2 = server.HttpClient.PutAsJsonAsync("api/Ticket/5", data).Result;

                Assert.IsTrue(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.Created, response2.StatusCode);

                var result2 = response2.Content.ReadAsAsync<TicketDTO>().Result;

                Assert.AreEqual(data.Name, result2.Name);

                // get modified ticket
                HttpResponseMessage response3 = server.HttpClient.GetAsync("api/Ticket/5").Result;

                Assert.IsTrue(response3.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response3.StatusCode);

                var data2 = response3.Content.ReadAsAsync<TicketDTO>().Result;

                Assert.AreEqual(data2.Name, data.Name);
                Assert.AreEqual(data.Sessions.Count(), result2.Sessions.Count());

            }

        }

        [TestMethod]
        public void Delete_ShouldRemoveTicket()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.DeleteAsync("api/Ticket/4").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);

                var result = response.Content.ReadAsAsync<TicketDTO>().Result;

                HttpResponseMessage response2 = server.HttpClient.GetAsync("api/Ticket/4").Result;

                Assert.IsFalse(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response2.StatusCode);

                //var result2 = response2.Content.ReadAsAsync<TicketDTO>().Result;
            }
        }

    }
}

