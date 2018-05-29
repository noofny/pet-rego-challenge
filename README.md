# PetRego Technical Challenge

<img src="./pics/logo-petshop.png">

This project is a response to a technical challenge. It is by no means a production system, however is intended as a demonstration of my ability to respond to a specific brief and stretch my technical capabilities within a limited timeframe.

<img height="100px" src="./pics/logo-brief.png">

## The Brief

Your client PetRego is looking to build a new service presenting a REST API that will allow it to maintain a list of owners, their pets, and attributes.

They want to record such details as:

- The owner's name
- The pet's name
- The type of animal (we particularly care about dogs, cats, chickens and snakes).

The most common request will be to look up the pets that belong to a person.

Months later after the success of PetRego's new service they have set up a partnership with a local pet food supplier that wants to use this information to start determining the food orders owners will need to make. In the time between release of Version 1 of your API and now, several third parties have built their own applications on top your API. PetRego does not want to break existing compatibility while introducing this new functionality.

We know that:

- Dogs eat bones
- Cats eat fish
- Chickens eat corn
- Snakes eat mice

Assessment Criteria:

- Build a simple rest service for storage and retrieval of owner and pet details. Your API should be:
  - RESTful.
  - Return (at least) JSON responses.
  - Unauthenticated (though if you have thoughts on that you can put it in comments).
- Your solution should be provided as a git repository. Please demonstrate regular commits and good git practices.
- Include a README with documentation around API endpoints and response format.
- Include in your README the commands for building and running your solution and the port it starts on.

Things we will be looking for:

- Data modelling and API design.
- Style:
  - Clarity
  - Commenting where appropriate
  - Consistency
- Testing.

Things that you should consider implementing in your solution:

- Versioned API
- Dependency Injection
- Mocking
- HATEOAS/Hypermedia Principles

Some notes:

- We realize that projects like this are never quite finished. We're not looking to take your assessment and put it into production.

- Take the time to demonstrate understanding of concepts; comments and TODOs are a great way to indicate what could be achieved in future for areas where the time/benefit trade off is not worth it.

- Don't over engineer the solution. Conversely, if there's a great tool that can auto-generate this entire solution from a YAML file, that's not showing us you understand the principles at work. For the sake of the assessment, you're welcome to use external dependencies, but don't go overboard.

<img height="100px" src="./pics/logo-idea.jpg">

## The Approach

Todo.

### Decisions & Challenges

- I chose to use a containerized ELK stack (ElasticSearch, Lucene, Kibana) for the data persistence layer. Possibly a contentious decision, I chose this as a learning exercise and to provide some fun with data visualizations which are available "out of the box" with Kibana.
- DotNetCore - purely as a learning exercise.
- Mocking IElasticClient in my repositories proved to be quite difficult, due to combination of using async methods coupled with limitations (or lack of docs/examples) around async handling within the mocking framework I used (Moq). In order not to have this slow the project I flagged as a todo and have moved on.

### Architecture

Todo.

### Assumptions, Notes and Stuff

- An assumption was made that it is safe for error/exception detail to be returned to failed API calls. In most production scenarios, it is often wise to abstract this detail and provide a standardized set of error codes which can be referenced via API documentation. This is often done sue to the high chance of inadvertently exposing personally identifiable information (PII) or other sensitive data via exceptions/stack traces etc.


## Setup

<img height="100px" src="./pics/logo-setup.png">

Todo.

## Usage

Todo.

## Future Improvements

Todo.

## References & Inspiration

- [HATEOAS Wiki](https://en.wikipedia.org/wiki/HATEOAS)
- [RFC5988 (web linking) @ IETF ](https://tools.ietf.org/html/rfc5988)
- [Link Relations](https://www.iana.org/assignments/link-relations/link-relations.xml)
- [existing rel values](http://microformats.org/wiki/existing-rel-values)
- [Generating Hypermedia links in ASP.NET Web API](http://benfoster.io/blog/generating-hypermedia-links-in-aspnet-web-api)
- [Building Hypermedia Web APIs with ASP.NET Web API](https://msdn.microsoft.com/en-us/magazine/jj883957.aspx)
- [Hypermedia links with Servicestack new API](https://stackoverflow.com/questions/18351944/hypermedia-links-with-servicestack-new-api)
- [HTTP status code for update and delete?](https://stackoverflow.com/questions/2342579/http-status-code-for-update-and-delete)
- [http-decision-diagram](https://github.com/for-GET/http-decision-diagram)
- [Postman Docs - Dynamic Variables](https://www.getpostman.com/docs/v6/postman/environments_and_globals/variables#dynamic-variables)
