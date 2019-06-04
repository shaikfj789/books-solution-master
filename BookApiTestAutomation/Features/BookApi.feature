Feature: BooksApi
	So that I can consume correct book metadata
	As a consumer
	I want to be able to perform all the operations

Background: 
Given I am requesting book metadata


	Scenario: GET specific books based on Title 
	Given I have the following books: 
	| Id | Title     | Description | Author |
	| 1	 | TestTitle1 | TestDescription1 | TestAuthor1 |
	| 2  | TestTitle2 | TestDescription2 | TestAuthor2 |
	| 3	 | TestTitle3 | TestDescription3 | TestAuthor3 |
	And I am requesting get a book endpoint for title "Test"
	When I make a request
	Then the response should include books
	And the response should include corresponding title
	And each book should include the field "Id"
	And each book should include the field "Title"
	And each book should include the field "Description"
	And each book should include the field "Author"
	And the response code should be 200

	Scenario: GET all books 
	Given I have the following books: 
	| Id | Title     | Description | Author |
	| 4	 | TestTitle4 | TestDescription4 | TestAuthor4 |
	| 5  | TestTitle5 | TestDescription5 | TestAuthor5 |
	And I am requesting all books endpoint
	When I make a request
	Then the response should include books
	And each book should include the field "Id"
	And each book should include the field "Title"
	And the response code should be 200

	Scenario: GET book information with Id
	Given I have the following books: 
	| Id | Title     | Description | Author |
	| 6	 | TestTitle6 | TestDescription6 | TestAuthor6 |
	And I am requesting get a book endpoint for Id 6
	When I make a request 
	Then there should be 6 , TestTitle6 , TestDescription6 and TestAuthor6 in the response
	And the response code should be 200

	Scenario: Add a new Book 
	Given there is no book with Id 7
	And I am adding a book	 
	When I make a call with the json table data:
	| Id | Title       | Description       | Author       |
	| 7 | TestTitle7 | TestDescription7 | TestAuthor7 |
	Then there should be 7 , TestTitle7 , TestDescription7 and TestAuthor7 in the response
	And the response code should be 200

	
	Scenario: Update an existing Book
	Given I have the following books: 
	| Id | Title     | Description | Author |
	| 8 | TestTitle8 | TestDescription8 | TestAuthor8 |
	And I am requesting update book endpoint for id 8
	When I make a call with the json data:
	"""
	{
	"Id": 8, "Title": "NewTitle", "Description": "NewDescription", "Author": "NewAuthor"
	}
	"""
	Then there should be 8 , NewTitle , NewDescription and NewAuthor in the response
	And the response code should be 200 

	
	Scenario: Delete an existing Book
	Given I have the following books: 
	| Id | Title     | Description | Author |
	| 9  | TestTitle9 | TestDescription9 | TestAuthor9 |
	And I am requesting delete book endpoint for id 9
	When I make a request
	Then the response code should be 204 	

	Scenario: Delete a non-existing Book
	Given there is no book with Id 121
	And I am requesting delete book endpoint for id 121
	When I make a request
	Then the response code should be 404
	And following message is displayed :
	"""
	{"Message":"Book with id 121 not found!"}
	"""
