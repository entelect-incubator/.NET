<img align="left" width="116" height="116" src="pezza-logo.png" />

# &nbsp;**Pezza - Phase 7**

<br/><br/>

# **UI**

In Phase 7 we will build the Front End implementations for the API in a variety of ways and technologies. * Remember to always be technology agnostic. Choose the technology that suites the use case the best.

## **Setup**

- [ ] Use the Final Solution from Phase 6 to get started or use Phase3\01. StartSolution
- [ ] To allow calls from your Web.API you need to add CORS in your starup.cs

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

## **Steps**

- [ ] [Step 1 - Dashboard (Pezza back office)](https://github.com/entelect-incubator/.NET/tree/master/Phase%203/02.%20Dashboard)
- [ ] [Step 2 - Website (Customers to order Pizza's)](https://github.com/entelect-incubator/.NET/tree/master/Phase%203/03.%20Website) 
 
