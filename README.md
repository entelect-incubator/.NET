# Welcome to .Net Incubator

### Learning Overview?

 - [ ] What is .Net?
 - [ ] Where can I ask for help?
 - [ ] Solving the most common problems found to solve
	 - [ ] CRUD System
	 - [ ] Handling Background Jobs
	 - [ ] Creating an API
		 - [ ] RESTful and
		 - [ ] gRPC
	 - [ ] Bulding a Front-End to consume your API - **your choice*
		 - [ ] Blazor or
		 - [ ] React or
		 - [ ] Angular

## Prerequirements

 - [ ] .NET Fundamentals - [Read more...](https://github.com/entelect-incubator/.Net/tree/master/01.%20Fundamentals)
 - [ ] .NET Overview - [Read more...](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-3.1&tabs=windows).
 
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

- [ ] Setup your enviroment - [How to video](https://www.youtube.com/watch?v=G1-Zfr9-3zs&list=PLLWMQd6PeGY2GVsQZ-u3DPXqwwKW8MkiP)
  - [ ] [Visual Studio 2019 Community](https://visualstudio.microsoft.com/downloads/) Installed
  - [ ] [.NET Core 3.1](https://dotnet.microsoft.com/download) Installed
  - [ ] [SQL Server Developer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) Installed
  - [ ] [SQL Server Management Studio SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) Installed
 - [ ] SQL Incubator - [SQL basics for Beginners](https://www.youtube.com/watch?v=9Pzj7Aj25lw)

# Stock Management

In this section you will be building a back office project to allow Pezza too manage there stock.

## Learning Outcomes

### Phase 1 

Admin should be able to manage the stock through a web application and manage the different restaurants. Also, a web application to allow a customer to order a pizza. We will solve this by doing the following:

 - Create a CRUD System in .Net MVC Project too manage stock and restaurants. Allow restaurants to place a request stock from head office.
 - Expose your Stock Management through an API using .Net Web API that will be used by the forn-end application.
 - Create a Customer Facing Website in your choice of Front-End Library.
   - AngularJS
   - ReactJS
   - Blazor
  
 <div class="mxgraph" style="max-width:100%;border:1px solid transparent;" data-mxgraph="{&quot;highlight&quot;:&quot;#0000ff&quot;,&quot;nav&quot;:true,&quot;resize&quot;:true,&quot;toolbar&quot;:&quot;zoom layers lightbox&quot;,&quot;edit&quot;:&quot;_blank&quot;,&quot;xml&quot;:&quot;&lt;mxfile host=\&quot;c13cad62-a6ef-4173-a00e-b8c77d094b2f\&quot; modified=\&quot;2020-08-01T13:27:29.214Z\&quot; agent=\&quot;5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Code/1.47.3 Chrome/78.0.3904.130 Electron/7.3.2 Safari/537.36\&quot; etag=\&quot;AoOgEdhMBcQtsDOmAtqS\&quot; version=\&quot;13.1.3\&quot;&gt;&lt;diagram id=\&quot;6hGFLwfOUW9BJ-s0fimq\&quot; name=\&quot;Page-1\&quot;&gt;&lt;mxGraphModel dx=\&quot;993\&quot; dy=\&quot;802\&quot; grid=\&quot;0\&quot; gridSize=\&quot;10\&quot; guides=\&quot;1\&quot; tooltips=\&quot;1\&quot; connect=\&quot;1\&quot; arrows=\&quot;1\&quot; fold=\&quot;1\&quot; page=\&quot;1\&quot; pageScale=\&quot;1\&quot; pageWidth=\&quot;827\&quot; pageHeight=\&quot;1169\&quot; background=\&quot;none\&quot; math=\&quot;0\&quot; shadow=\&quot;0\&quot;&gt;&lt;root&gt;&lt;mxCell id=\&quot;0\&quot;/&gt;&lt;mxCell id=\&quot;1\&quot; parent=\&quot;0\&quot;/&gt;&lt;mxCell id=\&quot;21\&quot; style=\&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;entryX=0.5;entryY=1;entryDx=0;entryDy=0;\&quot; edge=\&quot;1\&quot; parent=\&quot;1\&quot; source=\&quot;9\&quot; target=\&quot;4\&quot;&gt;&lt;mxGeometry relative=\&quot;1\&quot; as=\&quot;geometry\&quot;&gt;&lt;Array as=\&quot;points\&quot;&gt;&lt;mxPoint x=\&quot;274\&quot; y=\&quot;460\&quot;/&gt;&lt;mxPoint x=\&quot;190\&quot; y=\&quot;460\&quot;/&gt;&lt;/Array&gt;&lt;/mxGeometry&gt;&lt;/mxCell&gt;&lt;mxCell id=\&quot;22\&quot; style=\&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;exitX=0.5;exitY=0;exitDx=0;exitDy=0;entryX=0.5;entryY=1;entryDx=0;entryDy=0;\&quot; edge=\&quot;1\&quot; parent=\&quot;1\&quot; source=\&quot;9\&quot; target=\&quot;6\&quot;&gt;&lt;mxGeometry relative=\&quot;1\&quot; as=\&quot;geometry\&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=\&quot;9\&quot; value=\&quot;Pezza DB\&quot; style=\&quot;shape=datastore;whiteSpace=wrap;html=1;\&quot; vertex=\&quot;1\&quot; parent=\&quot;1\&quot;&gt;&lt;mxGeometry x=\&quot;240\&quot; y=\&quot;480\&quot; width=\&quot;68.5\&quot; height=\&quot;70\&quot; as=\&quot;geometry\&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=\&quot;19\&quot; style=\&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;\&quot; edge=\&quot;1\&quot; parent=\&quot;1\&quot; source=\&quot;4\&quot; target=\&quot;9\&quot;&gt;&lt;mxGeometry relative=\&quot;1\&quot; as=\&quot;geometry\&quot;&gt;&lt;mxPoint x=\&quot;270\&quot; y=\&quot;470\&quot; as=\&quot;targetPoint\&quot;/&gt;&lt;Array as=\&quot;points\&quot;&gt;&lt;mxPoint x=\&quot;190\&quot; y=\&quot;460\&quot;/&gt;&lt;mxPoint x=\&quot;274\&quot; y=\&quot;460\&quot;/&gt;&lt;/Array&gt;&lt;/mxGeometry&gt;&lt;/mxCell&gt;&lt;mxCell id=\&quot;4\&quot; value=\&quot;Admin Portal&amp;lt;br&amp;gt;Stock Management&amp;lt;br&amp;gt;- MVC\&quot; style=\&quot;whiteSpace=wrap;html=1;\&quot; vertex=\&quot;1\&quot; parent=\&quot;1\&quot;&gt;&lt;mxGeometry x=\&quot;140\&quot; y=\&quot;350\&quot; width=\&quot;100\&quot; height=\&quot;70\&quot; as=\&quot;geometry\&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=\&quot;18\&quot; style=\&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;entryX=0.5;entryY=0;entryDx=0;entryDy=0;\&quot; edge=\&quot;1\&quot; parent=\&quot;1\&quot; source=\&quot;6\&quot; target=\&quot;9\&quot;&gt;&lt;mxGeometry relative=\&quot;1\&quot; as=\&quot;geometry\&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=\&quot;20\&quot; style=\&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;entryX=0.5;entryY=1;entryDx=0;entryDy=0;\&quot; edge=\&quot;1\&quot; parent=\&quot;1\&quot; source=\&quot;6\&quot; target=\&quot;3\&quot;&gt;&lt;mxGeometry relative=\&quot;1\&quot; as=\&quot;geometry\&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=\&quot;6\&quot; value=\&quot;Web.Api\&quot; style=\&quot;whiteSpace=wrap;html=1;\&quot; vertex=\&quot;1\&quot; parent=\&quot;1\&quot;&gt;&lt;mxGeometry x=\&quot;280\&quot; y=\&quot;400\&quot; width=\&quot;100\&quot; height=\&quot;40\&quot; as=\&quot;geometry\&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=\&quot;17\&quot; style=\&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;entryX=0.5;entryY=0;entryDx=0;entryDy=0;\&quot; edge=\&quot;1\&quot; parent=\&quot;1\&quot; source=\&quot;3\&quot; target=\&quot;6\&quot;&gt;&lt;mxGeometry relative=\&quot;1\&quot; as=\&quot;geometry\&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=\&quot;3\&quot; value=\&quot;Customer Website\&quot; style=\&quot;whiteSpace=wrap;html=1;\&quot; vertex=\&quot;1\&quot; parent=\&quot;1\&quot;&gt;&lt;mxGeometry x=\&quot;280\&quot; y=\&quot;320\&quot; width=\&quot;100\&quot; height=\&quot;40\&quot; as=\&quot;geometry\&quot;/&gt;&lt;/mxCell&gt;&lt;/root&gt;&lt;/mxGraphModel&gt;&lt;/diagram&gt;&lt;/mxfile&gt;&quot;}"></div>
<script type="text/javascript" src="https://app.diagrams.net/js/viewer.min.js"></script>
 
 ### Phase 2
 
Now that we have deployed phase 1, we can make a few enhancements. Also it will be easier for the customer and admin to search and filter through the stock, so we will add that in as well. 
 
 Improve Form Validation
  - Fluent
 Filtering Searching Pagination
 
 ### Phase 3 
 
 When we work as part of a team, we usually need to adhere to coding standards. Let's have a look at how we can enforce some of the most basic standards.
  
 Coding Standards
  
 ### Phase 4
 
 The sites has been running for a while now and we need to look at how we can improve the performance next. 
 
  Increasing Performance
  - Caching
  - Repsonse COmpression
  	- Brotli
  - Transient vs Persitenet 
  
  ### Phase 5
 
 Now that we have increased the performance lets improve on the security 
 
  Add Security
  - API Oauth / JWT Token
  - MVC Antiforgy Tokens
 
 ### Phase 4
 
 
 
 Real time tracking
 
 
 

Advanced

# Customer Website

In this section you will be building a customer facing project to allow customers to browse Comic Stock comic books.

## Learning Outcomes

Basic

 - 
 - 

Advanced

 - Track a online order through using .Net gPRC.
 
 ### Phase 2
 
 Create a project to import bulk Stock from a File.
