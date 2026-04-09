# ThreadPilot

## Architecture and Design Decisions, Extensibility and Error Handling
Since it's the start of something that might grow the focus is to make it simple, extensible and maintainable. The project is structured in a way that separates concerns and allows for easy addition of new features in the future. Using dependency injection makes it easy to build test components now and substitue them for real ones, once it's been decided upon. Same thing with my choice of using repository pattern with mocked data, which allows for quick development and testing without the need for a database setup.
Depending on the future requirements I'd go with either a microservices architecture or a service architecture which this setup supports by either just exetending the seperate projects with thier own persistance or creating a commong infrastructure project which implements the repositories of both. The microservices architecture would allow for more flexibility and scalability, while the service architecture would be simpler to implement and maintain. Both architectures would allow for easy addition of new features and services in the future.
For this assignment, I chose to implement a simple error handling mechanism using try-catch blocks in the controllers. This allows me to catch any exceptions that may occur and return appropriate error responses to the client. In a real-world application, I would consider implementing a more robust error handling mechanism, such as using a global exception handler or implementing a retry mechanism for failed operations.

## How to run
1. Clone the repository.
1. Build the solution.
1. Set startup projects to ThreadPilot.Insurance and ThreadPilot.Vehicle.
1. Run the projects and use the ThreadPilot.Insurance.http and ThreadPilot.Vehicle.http files to test the API endpoints (or Postman or any other API testing tool).

## Personal Reflection
Nothing was particularly challenging, but I found it interesting to very quickly create the small project and think about how to make it extensible for future features while remembering to add as many vital parts as possible in a short time.
I have worked on several projects where I had to design and implement a RESTful API using ASP.NET Core, and I have experience with Entity Framework Core for data access. I have also worked on projects that required me to design the applications that needed to be easily changed and/or extensible for future features.
If I had more time I'd start by implementing:
- Added some security features, such as authentication and authorization, to protect the API endpoints and ensure that only authorized users can access the data.
- Implementing a more robust error handling mechanism, such as using a global exception handler and Problem Details.
- Added SqLite or another lightweight database to persist the data instead of using in-memory collections.