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
  
 <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" width="241px" viewBox="-0.5 -0.5 241 231" content="&lt;mxfile host=&quot;c13cad62-a6ef-4173-a00e-b8c77d094b2f&quot; modified=&quot;2020-08-01T13:28:44.784Z&quot; agent=&quot;5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Code/1.47.3 Chrome/78.0.3904.130 Electron/7.3.2 Safari/537.36&quot; etag=&quot;aBdfunUQN8NCk2p-J3zO&quot; version=&quot;13.1.3&quot;&gt;&lt;diagram id=&quot;6hGFLwfOUW9BJ-s0fimq&quot; name=&quot;Page-1&quot;&gt;&lt;mxGraphModel dx=&quot;993&quot; dy=&quot;802&quot; grid=&quot;0&quot; gridSize=&quot;10&quot; guides=&quot;1&quot; tooltips=&quot;1&quot; connect=&quot;1&quot; arrows=&quot;1&quot; fold=&quot;1&quot; page=&quot;1&quot; pageScale=&quot;1&quot; pageWidth=&quot;827&quot; pageHeight=&quot;1169&quot; background=&quot;none&quot; math=&quot;0&quot; shadow=&quot;0&quot;&gt;&lt;root&gt;&lt;mxCell id=&quot;0&quot;/&gt;&lt;mxCell id=&quot;1&quot; parent=&quot;0&quot;/&gt;&lt;mxCell id=&quot;21&quot; style=&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;entryX=0.5;entryY=1;entryDx=0;entryDy=0;&quot; edge=&quot;1&quot; parent=&quot;1&quot; source=&quot;9&quot; target=&quot;4&quot;&gt;&lt;mxGeometry relative=&quot;1&quot; as=&quot;geometry&quot;&gt;&lt;Array as=&quot;points&quot;&gt;&lt;mxPoint x=&quot;274&quot; y=&quot;460&quot;/&gt;&lt;mxPoint x=&quot;190&quot; y=&quot;460&quot;/&gt;&lt;/Array&gt;&lt;/mxGeometry&gt;&lt;/mxCell&gt;&lt;mxCell id=&quot;22&quot; style=&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;exitX=0.5;exitY=0;exitDx=0;exitDy=0;entryX=0.5;entryY=1;entryDx=0;entryDy=0;&quot; edge=&quot;1&quot; parent=&quot;1&quot; source=&quot;9&quot; target=&quot;6&quot;&gt;&lt;mxGeometry relative=&quot;1&quot; as=&quot;geometry&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=&quot;9&quot; value=&quot;Pezza DB&quot; style=&quot;shape=datastore;whiteSpace=wrap;html=1;&quot; vertex=&quot;1&quot; parent=&quot;1&quot;&gt;&lt;mxGeometry x=&quot;240&quot; y=&quot;480&quot; width=&quot;68.5&quot; height=&quot;70&quot; as=&quot;geometry&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=&quot;19&quot; style=&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;&quot; edge=&quot;1&quot; parent=&quot;1&quot; source=&quot;4&quot; target=&quot;9&quot;&gt;&lt;mxGeometry relative=&quot;1&quot; as=&quot;geometry&quot;&gt;&lt;mxPoint x=&quot;270&quot; y=&quot;470&quot; as=&quot;targetPoint&quot;/&gt;&lt;Array as=&quot;points&quot;&gt;&lt;mxPoint x=&quot;190&quot; y=&quot;460&quot;/&gt;&lt;mxPoint x=&quot;274&quot; y=&quot;460&quot;/&gt;&lt;/Array&gt;&lt;/mxGeometry&gt;&lt;/mxCell&gt;&lt;mxCell id=&quot;4&quot; value=&quot;Admin Portal&amp;lt;br&amp;gt;Stock Management&amp;lt;br&amp;gt;- MVC&quot; style=&quot;whiteSpace=wrap;html=1;&quot; vertex=&quot;1&quot; parent=&quot;1&quot;&gt;&lt;mxGeometry x=&quot;140&quot; y=&quot;350&quot; width=&quot;100&quot; height=&quot;70&quot; as=&quot;geometry&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=&quot;18&quot; style=&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;entryX=0.5;entryY=0;entryDx=0;entryDy=0;&quot; edge=&quot;1&quot; parent=&quot;1&quot; source=&quot;6&quot; target=&quot;9&quot;&gt;&lt;mxGeometry relative=&quot;1&quot; as=&quot;geometry&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=&quot;20&quot; style=&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;entryX=0.5;entryY=1;entryDx=0;entryDy=0;&quot; edge=&quot;1&quot; parent=&quot;1&quot; source=&quot;6&quot; target=&quot;3&quot;&gt;&lt;mxGeometry relative=&quot;1&quot; as=&quot;geometry&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=&quot;6&quot; value=&quot;Web.Api&quot; style=&quot;whiteSpace=wrap;html=1;&quot; vertex=&quot;1&quot; parent=&quot;1&quot;&gt;&lt;mxGeometry x=&quot;280&quot; y=&quot;400&quot; width=&quot;100&quot; height=&quot;40&quot; as=&quot;geometry&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=&quot;17&quot; style=&quot;edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;entryX=0.5;entryY=0;entryDx=0;entryDy=0;&quot; edge=&quot;1&quot; parent=&quot;1&quot; source=&quot;3&quot; target=&quot;6&quot;&gt;&lt;mxGeometry relative=&quot;1&quot; as=&quot;geometry&quot;/&gt;&lt;/mxCell&gt;&lt;mxCell id=&quot;3&quot; value=&quot;Customer Website&quot; style=&quot;whiteSpace=wrap;html=1;&quot; vertex=&quot;1&quot; parent=&quot;1&quot;&gt;&lt;mxGeometry x=&quot;280&quot; y=&quot;320&quot; width=&quot;100&quot; height=&quot;40&quot; as=&quot;geometry&quot;/&gt;&lt;/mxCell&gt;&lt;/root&gt;&lt;/mxGraphModel&gt;&lt;/diagram&gt;&lt;/mxfile&gt;" onclick="(function(svg){var src=window.event.target||window.event.srcElement;while (src!=null&amp;&amp;src.nodeName.toLowerCase()!='a'){src=src.parentNode;}if(src==null){if(svg.wnd!=null&amp;&amp;!svg.wnd.closed){svg.wnd.focus();}else{var r=function(evt){if(evt.data=='ready'&amp;&amp;evt.source==svg.wnd){svg.wnd.postMessage(decodeURIComponent(svg.getAttribute('content')),'*');window.removeEventListener('message',r);}};window.addEventListener('message',r);svg.wnd=window.open('https://app.diagrams.net/?client=1&amp;lightbox=1&amp;edit=_blank');}}})(this);" style="cursor:pointer;max-width:100%;max-height:231px;"><defs/><g><path d="M 134.3 160 L 134.3 140 L 50 140 L 50 106.37" fill="none" stroke="#000000" stroke-miterlimit="10" pointer-events="stroke"/><path d="M 50 101.12 L 53.5 108.12 L 50 106.37 L 46.5 108.12 Z" fill="#000000" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><path d="M 134.25 160 L 134.3 140 L 190 140 L 190 126.37" fill="none" stroke="#000000" stroke-miterlimit="10" pointer-events="stroke"/><path d="M 190 121.12 L 193.5 128.12 L 190 126.37 L 186.5 128.12 Z" fill="#000000" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><path d="M 100 169 C 100 157 168.5 157 168.5 169 L 168.5 221 C 168.5 233 100 233 100 221 Z" fill="#ffffff" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><path d="M 100 169 C 100 178 168.5 178 168.5 169 M 100 173.5 C 100 182.5 168.5 182.5 168.5 173.5 M 100 178 C 100 187 168.5 187 168.5 178" fill="none" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><g transform="translate(-0.5 -0.5)"><switch><foreignObject style="overflow: visible; text-align: left;" pointer-events="none" width="100%" height="100%" requiredFeatures="http://www.w3.org/TR/SVG11/feature#Extensibility"><div xmlns="http://www.w3.org/1999/xhtml" style="display: flex; align-items: unsafe center; justify-content: unsafe center; width: 67px; height: 1px; padding-top: 207px; margin-left: 101px;"><div style="box-sizing: border-box; font-size: 0; text-align: center; "><div style="display: inline-block; font-size: 12px; font-family: Helvetica; color: #000000; line-height: 1.2; pointer-events: all; white-space: normal; word-wrap: normal; ">Pezza DB</div></div></div></foreignObject><text x="134" y="210" fill="#000000" font-family="Helvetica" font-size="12px" text-anchor="middle">Pezza DB</text></switch></g><path d="M 50 100 L 50 140 L 134 140 L 134 153.63" fill="none" stroke="#000000" stroke-miterlimit="10" pointer-events="stroke"/><path d="M 134 158.88 L 130.5 151.88 L 134 153.63 L 137.5 151.88 Z" fill="#000000" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><rect x="0" y="30" width="100" height="70" fill="#ffffff" stroke="#000000" pointer-events="all"/><g transform="translate(-0.5 -0.5)"><switch><foreignObject style="overflow: visible; text-align: left;" pointer-events="none" width="100%" height="100%" requiredFeatures="http://www.w3.org/TR/SVG11/feature#Extensibility"><div xmlns="http://www.w3.org/1999/xhtml" style="display: flex; align-items: unsafe center; justify-content: unsafe center; width: 98px; height: 1px; padding-top: 65px; margin-left: 1px;"><div style="box-sizing: border-box; font-size: 0; text-align: center; "><div style="display: inline-block; font-size: 12px; font-family: Helvetica; color: #000000; line-height: 1.2; pointer-events: all; white-space: normal; word-wrap: normal; ">Admin Portal<br />Stock Management<br />- MVC</div></div></div></foreignObject><text x="50" y="69" fill="#000000" font-family="Helvetica" font-size="12px" text-anchor="middle">Admin Portal...</text></switch></g><path d="M 190 120 L 190 140 L 134.3 140 L 134.27 153.63" fill="none" stroke="#000000" stroke-miterlimit="10" pointer-events="stroke"/><path d="M 134.25 158.88 L 130.77 151.87 L 134.27 153.63 L 137.77 151.89 Z" fill="#000000" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><path d="M 190 80 L 190 46.37" fill="none" stroke="#000000" stroke-miterlimit="10" pointer-events="stroke"/><path d="M 190 41.12 L 193.5 48.12 L 190 46.37 L 186.5 48.12 Z" fill="#000000" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><rect x="140" y="80" width="100" height="40" fill="#ffffff" stroke="#000000" pointer-events="all"/><g transform="translate(-0.5 -0.5)"><switch><foreignObject style="overflow: visible; text-align: left;" pointer-events="none" width="100%" height="100%" requiredFeatures="http://www.w3.org/TR/SVG11/feature#Extensibility"><div xmlns="http://www.w3.org/1999/xhtml" style="display: flex; align-items: unsafe center; justify-content: unsafe center; width: 98px; height: 1px; padding-top: 100px; margin-left: 141px;"><div style="box-sizing: border-box; font-size: 0; text-align: center; "><div style="display: inline-block; font-size: 12px; font-family: Helvetica; color: #000000; line-height: 1.2; pointer-events: all; white-space: normal; word-wrap: normal; ">Web.Api</div></div></div></foreignObject><text x="190" y="104" fill="#000000" font-family="Helvetica" font-size="12px" text-anchor="middle">Web.Api</text></switch></g><path d="M 190 40 L 190 73.63" fill="none" stroke="#000000" stroke-miterlimit="10" pointer-events="stroke"/><path d="M 190 78.88 L 186.5 71.88 L 190 73.63 L 193.5 71.88 Z" fill="#000000" stroke="#000000" stroke-miterlimit="10" pointer-events="all"/><rect x="140" y="0" width="100" height="40" fill="#ffffff" stroke="#000000" pointer-events="all"/><g transform="translate(-0.5 -0.5)"><switch><foreignObject style="overflow: visible; text-align: left;" pointer-events="none" width="100%" height="100%" requiredFeatures="http://www.w3.org/TR/SVG11/feature#Extensibility"><div xmlns="http://www.w3.org/1999/xhtml" style="display: flex; align-items: unsafe center; justify-content: unsafe center; width: 98px; height: 1px; padding-top: 20px; margin-left: 141px;"><div style="box-sizing: border-box; font-size: 0; text-align: center; "><div style="display: inline-block; font-size: 12px; font-family: Helvetica; color: #000000; line-height: 1.2; pointer-events: all; white-space: normal; word-wrap: normal; ">Customer Website</div></div></div></foreignObject><text x="190" y="24" fill="#000000" font-family="Helvetica" font-size="12px" text-anchor="middle">Customer Website</text></switch></g></g><switch><g requiredFeatures="http://www.w3.org/TR/SVG11/feature#Extensibility"/><a transform="translate(0,-5)" xlink:href="https://desk.draw.io/support/solutions/articles/16000042487" target="_blank"><text text-anchor="middle" font-size="10px" x="50%" y="100%">Viewer does not support full SVG 1.1</text></a></switch></svg>
 
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
