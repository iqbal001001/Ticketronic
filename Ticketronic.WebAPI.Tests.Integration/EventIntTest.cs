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
    public class EventIntTest
    {
    

        [TestMethod]
        public void Get_ShouldReturnAllEvents()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                var client = server.HttpClient;
                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept
                //    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/Event").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<EventDTO>>().Result;
                Assert.AreEqual(3, result.Count());
            }
        }

        [Ignore]
        [TestMethod]
        public void Get_ShouldReturnAllEvents_page_pageSize()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.CreateRequest("api/Event?page=1&pageSize=2")
                                                        .AddHeader("header1", "headervalue1")
                                                        .GetAsync().Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<EventDTO>>().Result;
                Assert.AreEqual(3, result.Count());

                //HttpResponseMessage response2 = server.HttpClient.GetAsync("api/Event?page=2&pageSize=2").Result;

                //Assert.IsTrue(response2.IsSuccessStatusCode);
                //Assert.AreEqual(System.Net.HttpStatusCode.OK, response2.StatusCode);

                //var result2 = response2.Content.ReadAsAsync<List<EventDTO>>().Result;
                //Assert.AreEqual(1, result2.Count());

                // TODO : HTTPContext test for Pagination Object
            }
        }

        [TestMethod]
        public void Get_Single_ShouldReturnEvents()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Event/1").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<EventDTO>().Result;
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(1, result.Sessions.Count);
                Assert.AreEqual(1, result.Sessions[0].Id);
            }
        }

        [TestMethod]
        public void Post_ShouldInsertEvent()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                var sessions = new List<SessionDTO> { 
                    new SessionDTO { Name = "Session1", Date = DateTime.Now, Starttime = new TimeSpan(8, 00, 00) } 
                };
                var data = new EventDTO { Name = "Event D", Duration = 2, Sessions = sessions };
                HttpResponseMessage response = server.HttpClient.PostAsJsonAsync("api/Event", data).Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

                var result = response.Content.ReadAsAsync<EventDTO>().Result;
                Assert.AreEqual(data.Name, result.Name);
                Assert.AreEqual(data.Sessions.Count, result.Sessions.Count);
                Assert.AreEqual(data.Sessions[0].Name, result.Sessions[0].Name);
            }
        }

        [TestMethod]
        public void Put_ShouldUpdateEvent()
        {
            using (var server = TestServer.Create<MyStartup>())
            {

                HttpResponseMessage response1 = server.HttpClient.GetAsync("api/Event/3").Result;

                Assert.IsTrue(response1.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response1.StatusCode);

                var data = response1.Content.ReadAsAsync<EventDTO>().Result;
                data.Name = "Event CC";

                HttpResponseMessage response = server.HttpClient.PutAsJsonAsync("api/Event/3", data).Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

                var result = response.Content.ReadAsAsync<EventDTO>().Result;

                Assert.AreEqual(data.Name, result.Name);
            }

        }

        [TestMethod]
        public void Delete_ShouldRemoveEvent()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.DeleteAsync("api/Event/3").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);

                var result = response.Content.ReadAsAsync<EventDTO>().Result;

                HttpResponseMessage response2 = server.HttpClient.GetAsync("api/Event/3").Result;

                Assert.IsFalse(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response2.StatusCode);

                //var result2 = response2.Content.ReadAsAsync<EventDTO>().Result;
            }
        }

    }
}

