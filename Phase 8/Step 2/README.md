<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 8 - Step 2** [![.NET Core - Phase 6 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step2.yml)

<br/><br/>

## **Anti Forgery Token**

[Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-5.0)

## **Adding Anti Forgery Token to our Portal**

Use the solution from Step 1.

Make sure every Edit.cshtml and Create.cshtml has the following after the Html.BeginForm

```cs
@Html.AntiForgeryToken()
```

For example

```cs
@using (Html.BeginForm("Edit", "Product", FormMethod.Post, new { enctype = "multipart/form-data" }))
{
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(true)
```

Move to Phase 9
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%209)