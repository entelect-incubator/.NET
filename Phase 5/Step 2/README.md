<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 5 - Step 2** [![.NET Core - Phase 5 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase5-step2.yml)

<br/><br/>

## **Compression**

[Response Compression](https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-5.0)

To improve the response time we will be adding compression. There is a variety of other things you can do to improve performance, this is just one of them.

Open up Startup.cs in Pezza.API.

Add the following to the ConfigureServices method.

```cs
services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
services.AddResponseCompression();
```

Add the following to the Configure method.

```cs
app.UseResponseCompression();
```

Brotli compression is used by default if it is supported by the client. If Brotli is not supported, Gzip can be used if the client supports it.

## **Move to Phase 6**

[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%206) 