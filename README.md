# **Welcome to the .NET Incubator**

### **Learning Overview?**

- [ ] What is .Net?
- [ ] Where can I ask for help?
- [ ] Solving the most common problems found to solve
  - [ ] CRUD System
  - [ ] Handling Background Jobs
  - [ ] Creating an API
    - [ ] RESTful
  - [ ] Bulding a Front-End to consume your API - **your choice*
    - [ ] Blazor or
    - [ ] React or
    - [ ] Angular

## **Prerequirements**

- [ ] .NET Fundamentals - [Read more...](https://github.com/entelect-incubator/.Net/tree/master/01.%20Fundamentals)
- [ ] .NET Overview - [Read more...](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-3.1&tabs=windows)

For this Incubator we will be using [.NET Core 3.1](https://dotnet.microsoft.com/download) - Scan through the basic concepts.

    1. The Startup class
    2. Dependency injection (services)
    3. Middleware
    4. Servers
    5. Configuration
    6. Options
    7. Environments (dev, stage, prod)
    8. Logging
    9. Routing
    10. Handle errors
    11. Make HTTP requests
    12. Static files

### **Setup**

- [ ] Setup your enviroment - [How to video](https://www.youtube.com/watch?v=G1-Zfr9-3zs&list=PLLWMQd6PeGY2GVsQZ-u3DPXqwwKW8MkiP)
  - [ ] [Visual Studio 2019 Community](https://visualstudio.microsoft.com/downloads/) Installed
  - [ ] [.NET Core 3.1](https://dotnet.microsoft.com/download) Installed
  - [ ] [SQL Server Developer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) Installed
  - [ ] [SQL Server Management Studio SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) Installed
- [ ] SQL Incubator - [SQL basics for Beginners](https://www.youtube.com/watch?v=9Pzj7Aj25lw)

# **Pezza Digital Solutions**

In this section you will start building projects to allow Pezza too manage there stock and a front-end website where customers can order pizza online.

## **Learning Outcomes**

### **Phase 1**

Admin should be able to manage the stock through a web application as well as manage different restaurants. Customers should be able to order a pizza online, this order should be visible to the restaurant. The customer should also be notified that their pizza is on its way. We will start solving these business requirements by doing the following:

- Create a CRUD System in .Net MVC Project to manage stock and restaurants. Allow restaurants to place a request stock from head-office.
- Expose your Stock Management through an API using .Net Web API that will be used by the front-end application.
- Allow for customer notifications to be send out.
- Create a Customer Facing Website in your choice of Front-End Library.
  - AngularJS
  - ReactJS
  - Blazor
  
![Phase 1 High Level Design](./Assets/phase1-hld.svg)

[Click here to get started](https://github.com/entelect-incubator/.Net/tree/master/01.%20Fundamentals)

### **Phase 2**

Now that we have deployed phase 1, we can make a few enhancements. Also it will be easier for the customer and admin to search and filter through the stock, so we will add that in as well.

Improve how data is displayed and validated

- [ ] Fluent Validation
- [ ] Filtering
- [ ] Searching
- [ ] Pagination

### **Phase 3**

 When we work as part of a team, we usually need to adhere to coding standards. Let's have a look at how we can enforce some of the most basic standards.
  
 Coding Standards
  
### **Phase 4**

The sites has been running for a while now and we are getting a lot more customers on the site ordering pizza. Pezza has also expanded to more areas and realised the importance of memory and performance.

Increasing Performance

- [ ] Caching
- [ ] Compression
  
### **Phase 5**

Now that we have increased the performance lets and a increase of customers we need to secure our website.

Add Security
- [ ] API Oauth / JWT Token
- [ ] MVC Antiforgy Tokens

### **Phase 6**

We want customers to have the ability to track their orders.
- [ ] SignalR
