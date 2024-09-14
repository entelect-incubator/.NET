<img align="left" width="116" height="116" src="logo.png" />

# &nbsp;**E List - Phase 8** [![.NET - Phase 8 - Start Solution](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase8-startsolution.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase8-startsolution.yml)

<br/><br/>

# **Frontend**

In Phase 8 we will build the Front End implementations for the API in a variety of ways and technologies. \* Remember to always be technology agnostic. Choose the technology that suites the use case the best.

## **Setup**

-   [ ] Use the Final Solution from Phase 6 to get started or use Phase8\01. StartSolution
-   [ ] To allow calls from your Web.API you need to add CORS in your starup.cs

[About CORS](https://www.youtube.com/watch?v=UjozQOaGt1k)

public void ConfigureServices(IServiceCollection services)

```cs
services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)

```cs
app.UseCors("CorsPolicy");
```

## **Different .NET Frontend Types**

How to choose?

Choose **Blazor** when you want a full-stack C# experience, value component-based UI, and require real-time interactivity. Go for **MVC** when you prefer a well-established architecture, need strong cross-browser compatibility and SEO, or want more flexibility in your front-end choices.

Remember that each project is unique, so assessing your project's specific needs, your team's expertise, and the long-term goals of the application will guide your decision towards **Blazor**, **MVC**, or other suitable options.

For mobile choose [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui)

Different frontend technologies to choose from

### **Why choose MVC**

#### **Resources**

-   [ ] [MVC recommended tutorials and articles](https://learn.microsoft.com/en-us/aspnet/mvc/overview/getting-started/mvc-learning-sequence)

Sure, here's a blurb about why you might use the MVC (Model-View-Controller) architectural pattern in C#:

"The MVC (Model-View-Controller) architectural pattern is a powerful framework for structuring and organizing C# applications, particularly those with complex user interfaces and business logic. MVC promotes a clear separation of concerns, allowing developers to divide their code into three distinct components:

1. **Model:** The Model represents the application's data and business logic. It encapsulates the data manipulation, validation, and interactions with the database or external services. By isolating these concerns, changes to the data or logic don't necessarily affect the other parts of the application.

2. **View:** The View is responsible for displaying the data to the user. It represents the user interface elements and their arrangement. Separating the View from the Model allows for flexible user interface designs and makes it easier to update the UI without altering the underlying logic.

3. **Controller:** The Controller acts as an intermediary between the Model and the View. It handles user inputs, processes requests, and orchestrates the communication between the Model and the View. This separation ensures that the user interactions don't directly impact the data or UI rendering, enhancing maintainability and testability.

By adopting the MVC pattern in C#, developers can achieve a modular, organized, and maintainable codebase. This separation of concerns enhances collaboration among development teams, as different team members can work on different components without stepping on each other's toes. Additionally, MVC simplifies testing, as each component can be tested independently, leading to higher code quality and more efficient bug fixes. Overall, utilizing MVC in C# projects offers a robust foundation for creating scalable and adaptable applications."

#### **Practical**

-   [ ] [Step 1 - EList Dashboard](https://github.com/entelect-incubator/.NET/tree/master/Phase%208/Dashboard)
-   [ ] [Step 2 - EList Pizza Ordering Website for Customers](https://github.com/entelect-incubator/.NET/tree/master/Phase%208/ClientWebsite)

## Why Choose Blazor

#### **Resources**

-   [ ] [ASP.NET Core Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-7.0)

Blazor is a modern web framework developed by Microsoft that offers several compelling advantages for web application development:

1. **Single Language Stack:** With Blazor, you can build interactive web applications using C# and .NET instead of having to learn and use JavaScript.

2. **Code Reusability:** Blazor promotes code reusability between client-side and server-side components, leading to faster development and easier maintenance.

3. **Type Safety:** The statically typed nature of C# helps catch errors at compile-time, improving the robustness of applications.

4. **Full Integration with .NET Ecosystem:** Blazor seamlessly integrates with the broader .NET ecosystem, enabling developers to leverage existing knowledge and tools.

5. **Component-Based Architecture:** Following a component-based architecture akin to React or Angular, Blazor simplifies the creation and management of complex UI elements.

6. **Rapid Development:** Blazor's declarative syntax and component-based approach accelerate development by abstracting away low-level tasks.

7. **Real-time Interactivity:** Blazor supports real-time communication via SignalR, facilitating the creation of dynamic applications that update in response to data changes.

8. **Client-Side or Server-Side Rendering:** Blazor offers both client-side and server-side rendering options, accommodating various performance and SEO needs.

9. **Familiar Tooling:** If you're familiar with Visual Studio and other .NET tools, you'll find Blazor's toolset comfortable to work with.

10. **Strong Community and Support:** The growing Blazor community provides resources, tutorials, and community-driven libraries for assistance.

Ultimately, your choice to adopt Blazor should align with your project's needs, your team's skills, and your personal preferences. If you're already well-versed in C# and .NET and wish to build dynamic web applications with a consistent technology stack, Blazor could be an excellent fit.

#### **Practical**

-   [ ] [Step 1 - EList Dashboard](https://github.com/entelect-incubator/.NET/tree/master/Phase%208/Dashboard)
-   [ ] [Step 2 - EList Pizza Ordering Website for Customers](https://github.com/entelect-incubator/.NET/tree/master/Phase%208/ClientWebsite)
