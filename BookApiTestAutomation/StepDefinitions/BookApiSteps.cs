using Newtonsoft.Json;
using NUnit.Framework;
using BookApiTestAutomation.DataEntities;
using BookApiTestAutomation.Globals;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;
using TechTalk.SpecFlow;

namespace RestSharpBookApiTestAutomation.StepDefinitions
{
    [Binding]
    public class BooksSteps
    {
        private RestClient restClient;
        private RestRequest restRequest;
        private IRestResponse restResponse;
        private Book _book = new Book();
        private IList<Book> _books = new List<Book>();
        private string searchedTitle;

     
        [Given(@"I am requesting book metadata")]
        public void GivenIAmRequestingBookMetadata()
        {
            restClient = new RestClient(Constants.ApiBaseUrl); //http://localhost:9000/api
           
        }

        [Given(@"I have the following books:")]
        public void GivenIHaveTheFollowingBooks(Table table)
        {
            restRequest = new RestRequest(Constants.BooksEndPoint, Method.POST);
            restRequest.AddHeader("Accept", "application/json");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string JSONString = JsonConvert.SerializeObject(table.Rows[i]);
                restRequest.AddParameter("application/json", JSONString, ParameterType.RequestBody);
                restResponse = restClient.Execute(restRequest);
                Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
                restRequest.Parameters.Clear();
            }

        }

        [Given(@"I am requesting get a book endpoint for title ""(.*)""")]
        public void GivenIAmRequestingbooksEndpointForTitle(string booktitle)
        {
            searchedTitle = booktitle;
            restRequest = new RestRequest(Constants.BooksEndPoint + "?title={title}", Method.GET);
            restRequest.AddUrlSegment("title", booktitle);
        }

        [Given(@"I am requesting get a book endpoint for Id (.*)")]
        public void GivenIamRequestingBookEndpointForId(string bookid)
        {
         
            restRequest = new RestRequest(Constants.BooksEndPoint + "/{bookid}", Method.GET);

            restRequest.AddUrlSegment("bookid", bookid);
        }

        [Given(@"I am requesting all books endpoint")]
        public void GivenIAmRequestingAllbooksEndpoint()
        {
         
            restRequest = new RestRequest(Constants.BooksEndPoint, Method.GET);

        }

       
        [Given(@"I am adding a book")]
        public void GivenIAmAddingABook()
        {
            restRequest = new RestRequest(Constants.BooksEndPoint, Method.POST);
            restRequest.AddHeader("Accept", "application/json");
            //restRequest.Parameters.Clear();

        }
        [Given(@"there is no book with Id (.*)")]
        public void GivenThereIsNoBookWithId(string bookId)
        {
            restRequest = new RestRequest(Constants.BooksEndPoint + "/{bookId}", Method.GET);
            restRequest.AddUrlSegment("bookId", bookId);
            restResponse = restClient.Execute(restRequest);
            Assert.AreEqual(HttpStatusCode.NotFound, restResponse.StatusCode);
        }

        [Given(@"I am requesting update book endpoint for id (.*)")]
        public void GivenIAmRequestingUpdateABook(string bookId)
        {
            restRequest = new RestRequest(Constants.BooksEndPoint + "/{bookId}", Method.PUT);
            restRequest.AddUrlSegment("bookId", bookId);
            restRequest.AddHeader("Accept", "application/json");
        }
        [Given(@"I am requesting delete book endpoint for id(.*)")]
        public void GivenIAmRequestingDeleteABook(string bookId)
        {
            restRequest = new RestRequest(Constants.BooksEndPoint + "/{bookId}", Method.DELETE);
            restRequest.AddUrlSegment("bookId", bookId);
        }

        

        [When(@"I make a request")]
        public void WhenIMakeARequest()
        {
            restResponse = restClient.Execute(restRequest);
           
        }

     
        [When(@"A book with id (.*) exists")]
        public void GivenABookWithIdExists(int bookId)
        {

            Assert.AreEqual(bookId, _book.Id);
        }

        [When(@"I make a call with the json data:")]
        public void WhenIMakeARequestwithJsondata(string jsonData)
        {
            restRequest.AddParameter("application/json",jsonData, ParameterType.RequestBody);
            restResponse = restClient.Execute(restRequest);
            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
            
        }

        [When(@"I make a call with the json table data:")]
        public void WhenIMakeACallWithTheJsonTableData(Table table)
        {
            string JSONString = JsonConvert.SerializeObject(table.Rows[0]);
            restRequest.AddParameter("application/json", JSONString, ParameterType.RequestBody);
            restResponse = restClient.Execute(restRequest);
            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode);
            
        }

        [Then(@"the response should include books")]
        public void ThenTheResponseShouldIncludeBooks()
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            _books = jsonSerializer.Deserialize<IList<Book>>(restResponse.Content);
            Assert.AreNotEqual(0, _books.Count);
        }

        [Then(@"the response should include corresponding title")]
        public void ThenTheResponseShouldIncludeTitle()
        {
            Assert.IsNotEmpty(_books);
            foreach (var book in _books)
            {
                var actualTitle = book.Title;
                Assert.IsTrue(actualTitle != null && actualTitle.Contains(searchedTitle));
            }
        }

        [Then(@"each book should include the field ""(.*)""")]
        public void EachBookShouldIncludeTheField(string fieldName)
        {
            Assert.IsNotEmpty(_books);

            foreach (var book in _books)
            {
                var outcome = book.GetType().GetProperty(fieldName);
                Assert.IsNotNull(outcome);
            }
        }

        [Then(@"there should be (.*) , (.*) , (.*) and (.*) in the response")]
        public void ThenThereShouldBeTestTitleAndTestDescriptionInTheResponse(int expectedId, string expectedTitle, string expectedDescription,string expectedAuthor)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            _book = jsonSerializer.Deserialize<Book>(restResponse.Content);
            Assert.AreEqual(expectedId, _book.Id);
            Assert.AreEqual(expectedTitle, _book.Title);
            Assert.AreEqual(expectedDescription, _book.Description);
            Assert.AreEqual(expectedAuthor, _book.Author);

        }


        [Then(@"the response code should be (.*)")]
        public void ThenTheResponseShouldBe(int expectedStatusCode)
        {
            int actualStatusCode = (int) restResponse.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Then(@"following message is displayed :")]
        public void ThenFollowingMessageIsDisplayed(string expectedMsg)
        {
            string actualMsg = restResponse.Content;

            Assert.AreEqual(expectedMsg, actualMsg);

        }
        }
}
