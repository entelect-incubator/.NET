<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 5 - Step 2** [![.NET Core - Phase 5 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-step2.yml)

<br/><br/>

## **Compression**

[Response Compression](https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-5.0)

To improve the response time we will be adding compression. There is a variety of other things you can do to improve performance. There are just a few things you can do.

Open up Startup.cs in Pezza.API

public void ConfigureServices(IServiceCollection services)

```cs
services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
services.AddResponseCompression();
```

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)

```cs
app.UseResponseCompression();
```

## **Move to Phase 6**

[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%206) 