# ThreadPilot

## Architecture and Design Decisions

## How to run
1. Clone the repository.
1. Build the solution.
1. Set startup projects to ThreadPilot.Insurance and ThreadPilot.Vehicle.
1. Run the projects and use the ThreadPilot.Insurance.http and ThreadPilot.Vehicle.http files to test the API endpoints (or Postman or any other API testing tool).

## Error Handling

## Extensibility

## Personal Reflection

### Any similar project or experience you’ve had in the past.
Several projects where I had to design and implement a RESTful API using ASP.NET Core, and I have experience with Entity Framework Core for data access. I have also worked on projects that required me to design the architecture of the application and make it extensible for future features.
### What was challenging or interesting in this assignment.
Nothing was particularly challenging, but I found it interesting to design the architecture of the project and think about how to make it extensible for future features.
### What you would improve or extend if you had more time.
- I'd liked to separate the two main projects into three projects each, .Api, .Core and .Infrastructure.
- I would have built a deployment Pipeline for AzureDevOps.
- Implementing a more robust error handling mechanism, such as using a global exception handler or implementing a retry mechanism for failed operations.
- Added some security features, such as authentication and authorization, to protect the API endpoints and ensure that only authorized users can access the data.