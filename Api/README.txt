API consist of 2 projets.
One, named API, is containing API with DB connection, second, named ApiTest, is only for controller tests.

Api project is production project.
Project is consist of several folders divided by logical parts.
Controllers - Api Controllers
Filter - Pagination filter
Helpers - Pagination helper for pages 
Migrations - DB change migrations
Models - Api models 
Services - services to help API
Wrappers - wrappers for PagedResponse and Response

In ApiTest you can find ProductControllerTest class, where are test for Product controller.
Each test method is calling Setup which is preparing mock data. 
After setup is called logic inside of method and method is checking required result.