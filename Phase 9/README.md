# &nbsp;**Pezza - Phase 9 — Security** [![.NET - Phase 9](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase9-finalsolution.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase9-finalsolution.yml)

![Pezza logo](./pezza-logo.png "Pezza logo")

## Quick facts

- Estimated time: 4 - 8 hours
- Difficulty: Intermediate ▮▮▮▯▯ (3/5)
- Target SDK: .NET 10 (net10)
- Audience: Developers moving from other platforms to C# who need to secure web APIs and web apps

## Goal

This phase focuses on improving the security posture of the Pezza solution: authentication and authorization, anti-forgery, secure headers, HTTPS/HSTS, secrets management, and small hardening changes that make the solution safe for demos and local testing.

## Prerequisites

- Completed Phase 7 (Api & Api.Client available)
- .NET SDK 10 installed
- Basic knowledge of authentication concepts (JWT/OAuth2) and web security fundamentals

## What you'll do (high level)

1. Add or validate authentication (JWT/OAuth2) for the Api and protect endpoints.
2. Add antiforgery validation to MVC endpoints that accept browser POSTs.
3. Enforce HTTPS and HSTS for production environments.
4. Harden cookie settings, secure headers and secrets handling.
5. Run quick automated checks and add CI validations for build and basic security tests where possible.

## Quick checklist & snippets

1. Enforce HTTPS & HSTS (Program.cs / Startup)

```csharp
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
```

1. Secure cookie settings

```csharp
services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});
```

1. JWT authentication (example)

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });

app.UseAuthentication();
app.UseAuthorization();
```

1. Antiforgery for browser POSTs

```csharp
services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

[ValidateAntiForgeryToken]
public IActionResult PostOrder(OrderModel model) { ... }
```

1. Secure headers (minimal middleware)

```csharp
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    await next();
});
```

1. Secrets & data protection

- Use `dotnet user-secrets` for local dev secrets.
- Use a vault (Azure Key Vault, AWS Secrets Manager) for CI/production secrets.
- Persist data protection keys to a shared store in multi-instance deployments.

## Validate / Quick commands

```powershell
# check .NET version (should be 10.x)
dotnet --version

# build the Phase 8 solution
dotnet build "./Phase 8/src/01. StartSolution/Pezza.sln"

# run tests (if present)
dotnet test "./Phase 8/src/01. StartSolution/Pezza.sln"
```

## Outcomes / Learning goals

- Configure JWT/OAuth and protect API endpoints.
- Harden cookie policies, antiforgery and secure headers for web UIs.
- Understand secrets management options for dev and CI.

---

If you'd like, I can make a small, gated code change to the `Api` project's `Program.cs` to enable HTTPS/HSTS and add the secure-headers middleware behind an environment check — I will only do that if you ask me to create a PR.


[Move to Phase 10](https://github.com/entelect-incubator/.NET/tree/master/Phase%2010)
