<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 8 - Step 1** [![.NET 7 - Phase 8 - Step 1](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase8-step1.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase8-step1.yml)

<br/><br/>

We will learn how to setup JWT Authentication in our API.

## **JWT**

JWT has become more and more popular in web development. It is an open standard which allows transmitting data between parties as a JSON object in a secure and compact way. The data transmitting using JWT between parties are digitally signed so that it can be easily verified and trusted.

[Introduction to JWT](https://www.c-sharpcorner.com/article/introduction-to-jwt/)

The first step is to configure JWT based authentication in our project. To do this, we need to register a JWT authentication schema by using "AddAuthentication" method and specifying JwtBearerDefaults.AuthenticationScheme. Here, we configure the authentication schema with JWT bearer options.

Install Nuget
- [ ] Package Microsoft.AspNetCore.Authentication
- [ ] Microsoft.AspNetCore.Authentication.JwtBearer

Add to startup.cs

```cs
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = this.Configuration["Jwt:Issuer"],
            ValidAudience = this.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });
```

In this example, we have specified which parameters must be taken into account to consider JWT as valid. As per our code,  the following items consider a token valid:

- Validate the server (ValidateIssuer = true) that generates the token.
- Validate the recipient of the token is authorized to receive (ValidateAudience = true)
- Check if the token is not expired and the signing key of the issuer is valid (ValidateLifetime = true)
- Validate signature of the token (ValidateIssuerSigningKey = true)
- Additionally, we specify the values for the issuer, audience, signing key. In this example, I have stored these values in appsettings.json file.

AppSettings.json

```json
{    
  "Jwt": {    
    "Key": "MySecretPezzaKey",    
    "Issuer": "pezza.co.za"    
  }    
}  
```

The above-mentioned steps are used to configure a JWT based authentication service. The next step is to make the authentication service is available to the application. To do this, we need to call app.UseAuthentication() method in the Configure method of startup class. The UseAuthentication method is called before UseMvc method.

## **Generate JWT Token**

I have created a AuthController and Generate method within this controller, which is responsible to generate the JWT. I have marked this method with the AllowAnonymous attribute to bypass the authentication. This method expects the AuthModel object for API KEY.

API generates the new token by using the "GenerateJSONWebToken" method.

Here, I have created a JWT using the JwtSecurityToken class. I have created an object of this class by passing some parameters to the constructor such as issuer, audience, expiration, and signature.

Finally, JwtSecurityTokenHandler.WriteToken method is used to generate the JWT. This method expects an object of the JwtSecurityToken class.

AuthController.cs

```cs
namespace Api.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Common.Models;

[ApiController]
public class AuthController : ApiController
{
    private readonly IConfiguration config;

    public AuthController(IConfiguration config) => this.config = config;

    [AllowAnonymous]
    [HttpPost]
    [Route("Authorise")]
    public IActionResult Authorise([FromBody] AuthModel auth)
    {
        IActionResult response = this.Unauthorized();
        var authenticate = this.Authenticate(auth);

        if (authenticate.Succeeded)
        {
            var tokenString = this.GenerateJSONWebToken();
            response = this.Ok(new { token = tokenString });
        }

        return response;
    }

    private string GenerateJSONWebToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(this.config["Jwt:Issuer"], this.config["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Result Authenticate(AuthModel auth)
    {
        ////Demo Purpose, I have Passed HardCoded User Information
        if (auth.ApiKey == "940312b1cd649122b2f29fc2a68e47dbfaf12aca")
        {
            return Result.Success();
        }

        return Result.Failure("Validation Error");
    }
}
```

Add the following models into Common\Models

AuthModel.cs

```cs
namespace Common.Models;

public class AuthModel
{
    public string ApiKey { get; set; }
}
```

TokenModel.cs

```cs
namespace Common.Models;

public class TokenModel
{
    public string Token { get; set; }
}
```

Once, we have enabled the JWT based authentication, I have created a simple Web API method that returns a list of value strings when invoked with an HTTP GET request. Here, I have marked this method with the authorize attribute, so that this endpoint will trigger the validation check of the token passed with an HTTP request.

If we call this method without a token, we will get 401 (UnAuthorizedAccess) HTTP status code as a response. If we want to bypass the authentication for any method, we can mark that method with the AllowAnonymous attribute.

![](Assets/2021-05-10-21-20-57.png)

As a response, we will get the JSON like the following,

```json
{    
    "token" : "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJKaWduZXNoIFRyaXZlZGkiLCJlbWFpbCI6InRlc3QuYnRlc3RAZ21haWwuY29tIiwiRGF0ZU9mSm9pbmciOiIwMDAxLTAxLTAxIiwianRpIjoiYzJkNTZjNzQtZTc3Yy00ZmUxLTgyYzAtMzlhYjhmNzFmYzUzIiwiZXhwIjoxNTMyMzU2NjY5LCJpc3MiOiJUZXN0LmNvbSIsImF1ZCI6IlRlc3QuY29tIn0.8hwQ3H9V8mdNYrFZSjbCpWSyR1CNyDYHcGf6GqqCGnY"    
} 
```

Now, we will try to get the list of values by passing this token into the authentication HTTP header. Following is my Action method definition.

Add Authorize to all Controllers except AuthController and PictureController.  For example 

```cs
[ApiController]
[Authorize]
public class CustomerController
```

Add Authentication to Swagger

Startup.cs

```cs
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pezza API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }, new string[] { }
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
```

Test Using Swagger

![](Assets/2021-05-10-21-49-30.png)

![](Assets/2021-05-10-21-48-36.png)

## **MVC**

Add Authorization Bearer in your API Call Header with generated JWT Token to test with


Modify ApiCallHelper.cs to include BearerToken.

SetBearerToken() get's the latest JWT Token

```cs
private async Task<string> SetBearerToken()
{
    var json = JsonSerializer.Serialize(new AuthModel
    {
        ApiKey = "940312b1cd649122b2f29fc2a68e47dbfaf12aca"
    });
    var data = new StringContent(json, Encoding.UTF8, "application/json");

    var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Auth/Authorise", data);
    if (responseMessage.IsSuccessStatusCode)
    {
        var responseData = await responseMessage.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<TokenModel>(responseData, this.jsonSerializerOptions);
        return response.Token;
    }

    return string.Empty;
}
```


On very call add Authorization Header

this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

Full Code

```cs
namespace Portal.Helpers;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Common;
using Common.Models;

public class ApiCallHelper<T>
{
    private readonly IHttpClientFactory clientFactory;

    private readonly HttpClient client;

    public List<ValidationError> ValidationErrors;

    private readonly JsonSerializerOptions jsonSerializerOptions;

    public string ControllerName { get; set; }

    public ApiCallHelper(IHttpClientFactory clientFactory)
    {
        this.clientFactory = clientFactory;
        this.client = clientFactory.CreateClient();
        this.client.BaseAddress = new Uri(AppSettings.ApiUrl);
        this.client.DefaultRequestHeaders.Accept.Clear();
        this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        this.jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true,
            MaxDepth = 20
        };
    }

    private async Task<string> SetBearerToken()
    {
        var json = JsonSerializer.Serialize(new AuthModel
        {
            ApiKey = "940312b1cd649122b2f29fc2a68e47dbfaf12aca"
        });
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}Auth/Authorise", data);
        if (responseMessage.IsSuccessStatusCode)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<TokenModel>(responseData, this.jsonSerializerOptions);
            return response.Token;
        }

        return string.Empty;
    }

    public async Task<ListOutcome<T>> GetListAsync(T entity)
    {
        this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

        var json = JsonSerializer.Serialize(entity);
        var response = await this.client.PostAsync(@$"{AppSettings.ApiUrl}{ControllerName}\Search", new StringContent(json, Encoding.UTF8, "application/json"));
        var responseData = await response.Content.ReadAsStringAsync();

        var entities = JsonSerializer.Deserialize<ListOutcome<T>>(responseData, this.jsonSerializerOptions);
        return entities;
    }

    public async Task<T> GetAsync(int id)
    {
        this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

        var responseMessage = await this.client.GetAsync(@$"{AppSettings.ApiUrl}{ControllerName}\{id}");
        if (responseMessage.IsSuccessStatusCode)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);
        }

        return default;
    }

    public async Task<Result<T>> Create(T entity)
    {
        this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

        this.ValidationErrors = new List<ValidationError>();
        var json = JsonSerializer.Serialize(entity);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var responseMessage = await this.client.PostAsync(@$"{AppSettings.ApiUrl}{ControllerName}", data);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<Result>(responseData, this.jsonSerializerOptions);

            this.ValidationErrors = response.Errors.Select(x =>
            {
                return (x as JObject).ToObject<ValidationError>();
            }).ToList();

            return Result<T>.Failure("ValidationError");
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);

            return Result<T>.Success(response);
        }

        return Result<T>.Failure("Error");
    }

    public async Task<Result<T>> Edit(T entity)
    {
        this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

        this.ValidationErrors = new List<ValidationError>();
        var json = JsonSerializer.Serialize(entity);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var responseMessage = await this.client.PutAsync(@$"{AppSettings.ApiUrl}{ControllerName}", data);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<Result>(responseData, this.jsonSerializerOptions);

            this.ValidationErrors = response.Errors?.Select(x =>
            {
                return (x as JObject).ToObject<ValidationError>();
            }).ToList();

            return Result<T>.Failure("ValidationError");
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<T>(responseData, this.jsonSerializerOptions);

            return Result<T>.Success(response);
        }

        return Result<T>.Failure("Error");
    }

    public async Task<bool> Delete(int id)
    {
        this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await this.SetBearerToken());

        var responseMessage = await this.client.DeleteAsync(@$"{AppSettings.ApiUrl}{ControllerName}\{id}");
        if (responseMessage.IsSuccessStatusCode)
        {
            var responseData = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<bool>(responseData, this.jsonSerializerOptions);
        }

        return false;
    }
}
```

## **STEP 2 - AntiForgeryToken**

Move to Step 2
[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%208/Step%202)