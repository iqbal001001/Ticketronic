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
    public class TransactionIntTest
    {

        [TestMethod]
        public void Get_ShouldReturnAllTransactions()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                var client = server.HttpClient;
                HttpResponseMessage response = client.GetAsync("api/Transaction").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<TransactionDTO>>().Result;
                Assert.AreEqual(3, result.Count());
            }
        }

        [TestMethod]
        public void Get_ShouldReturnAllTransactions_page_pageSize()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.CreateRequest("api/Transaction?page=1&pageSize=2")
                                                        .AddHeader("header1", "headervalue1")
                                                        .GetAsync().Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<TransactionDTO>>().Result;
                Assert.AreEqual(3, result.Count());

                //HttpResponseMessage response2 = server.HttpClient.GetAsync("api/Transaction?page=2&pageSize=2").Result;

                //Assert.IsTrue(response2.IsSuccessStatusCode);
                //Assert.AreEqual(System.Net.HttpStatusCode.OK, response2.StatusCode);

                //var result2 = response2.Content.ReadAsAsync<List<TransactionDTO>>().Result;
                //Assert.AreEqual(1, result2.Count());

                // TODO : HTTPContext test for Pagination Object
            }
        }

        [TestMethod]
        public void Get_Single_ShouldReturnTransactions()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Transaction/1").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<TransactionDTO>().Result;
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(1, result.Tickets.Count);
                Assert.AreEqual(1, result.Tickets[0].Id);
            }
        }

        [TestMethod]
        public void Post_ShouldInsertTransaction()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                // get ticket
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Ticket/1").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var ticket = response.Content.ReadAsAsync<TicketDTO>().Result;
                Assert.AreEqual(1, ticket.Id);
                Assert.AreEqual(1, ticket.Sessions.Count);
                Assert.AreEqual(1, ticket.Sessions[0].Id);

                //set up
                var data = new TransactionDTO { Email = "C4@domain.com", ContactNo = "0123456789", FirstName = "C4", LastName = "C4",
                    Tickets = new List<TicketPurchaseDTO> {
                        new TicketPurchaseDTO { Quantity = 1 , TicketId = ticket.Id} 
                    }
                };

                HttpResponseMessage response2 = server.HttpClient.PostAsJsonAsync("api/Transaction", data).Result;

                Assert.IsTrue(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.Created, response2.StatusCode);

                var result2 = response2.Content.ReadAsAsync<TransactionDTO>().Result;
                Assert.AreEqual(data.Email, result2.Email);
                Assert.AreEqual(data.Tickets.Count, result2.Tickets.Count);
                Assert.AreEqual(data.Tickets[0].TicketId, result2.Tickets[0].TicketId);
            }
        }

        [TestMethod]
        public void Put_ShouldUpdateTransaction_With_AddTicketsPuchased()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                // get ticket
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Ticket/1").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var ticket = response.Content.ReadAsAsync<TicketDTO>().Result;
                Assert.AreEqual(1, ticket.Id);
                Assert.AreEqual(1, ticket.Sessions.Count);
                Assert.AreEqual(1, ticket.Sessions[0].Id);

                // get original transaction
                HttpResponseMessage response1 = server.HttpClient.GetAsync("api/Transaction/3").Result;

                Assert.IsTrue(response1.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response1.StatusCode);

                var data = response1.Content.ReadAsAsync<TransactionDTO>().Result;
                data.FirstName = "Transaction CC1";
                data.Tickets.Add(new TicketPurchaseDTO { Quantity = 1, TicketId = ticket.Id });

                // update transaction 
                HttpResponseMessage response2 = server.HttpClient.PutAsJsonAsync("api/Transaction/3", data).Result;

                Assert.IsTrue(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.Created, response2.StatusCode);

                var result = response2.Content.ReadAsAsync<TransactionDTO>().Result;

                Assert.AreEqual(data.FirstName, result.FirstName);

                // get modified transaction
                HttpResponseMessage response3 = server.HttpClient.GetAsync("api/Transaction/3").Result;

                Assert.IsTrue(response3.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response3.StatusCode);

                var data2 = response3.Content.ReadAsAsync<TransactionDTO>().Result;
                Assert.AreEqual(data2.FirstName, data.FirstName);
                Assert.AreEqual(data.Tickets.Count(), data.Tickets.Count());
              
            }

        }

        [TestMethod]
        public void Put_ShouldUpdateTransaction_With_RemoveTicketsPuchased()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                // get original transaction
                HttpResponseMessage response1 = server.HttpClient.GetAsync("api/Transaction/3").Result;

                Assert.IsTrue(response1.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response1.StatusCode);

                var data = response1.Content.ReadAsAsync<TransactionDTO>().Result;
                data.FirstName = "Transaction CC1";
                data.Tickets.RemoveAt(0); // remove ticket

                // update transaction 
                HttpResponseMessage response2 = server.HttpClient.PutAsJsonAsync("api/Transaction/3", data).Result;

                Assert.IsTrue(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.Created, response2.StatusCode);

                var result = response2.Content.ReadAsAsync<TransactionDTO>().Result;

                Assert.AreEqual(data.FirstName, result.FirstName);

                // get modified transaction
                HttpResponseMessage response3 = server.HttpClient.GetAsync("api/Transaction/3").Result;

                Assert.IsTrue(response3.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response3.StatusCode);

                var data2 = response3.Content.ReadAsAsync<TransactionDTO>().Result;
                Assert.AreEqual(data2.FirstName, data.FirstName);
                Assert.AreEqual(data.Tickets.Count(), data.Tickets.Count());

            }

        }

        [TestMethod]
        public void Delete_ShouldRemoveTransaction()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.DeleteAsync("api/Transaction/3").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);

                var result = response.Content.ReadAsAsync<TransactionDTO>().Result;

                HttpResponseMessage response2 = server.HttpClient.GetAsync("api/Transaction/3").Result;

                Assert.IsFalse(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response2.StatusCode);

                //var result2 = response2.Content.ReadAsAsync<TransactionDTO>().Result;
            }
        }

    }
}

