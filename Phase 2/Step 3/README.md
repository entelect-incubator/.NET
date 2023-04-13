<img align="left" width="116" height="116" src="../Assets/pezza-logo.png" />

# &nbsp;**Pezza - Phase 2 - Step 3**

<br/><br/>

Finishing up the API to use CQRS

## **API**

In order to return clean unified responses consumers of the API, ActionResult Helper class. Depending on the data retrieved from the Core layer it will cater for the HTTP response and prevent duplicating code in controllers.

Create a Helpers folder in Api and ResponseHelper.cs inside it with the following code.

```cs
namespace Api.Helpers;

using Microsoft.AspNetCore.Mvc;
using Api.Controllers;
using Common.Models;

public static class ResponseHelper
{
    public static ActionResult ResponseOutcome<T>(Result<T> result, ApiController controller)
    {
        if (result.Data == null)
        {
            return controller.NotFound(Result.Failure( $"{typeof(T).Name.Replace("DTO","")} not found"));
        }

        if (!result.Succeeded)
        {
            return controller.BadRequest(result);
        }

        return controller.Ok(result);
    }

    public static ActionResult ResponseOutcome<T>(ListResult<T> result, ApiController controller)
    {
        if (!result.Succeeded)
        {
            return controller.BadRequest(result);
        }

        return controller.Ok(result);
    }

    public static ActionResult ResponseOutcome(Result result, ApiController controller)
    {
        if (!result.Succeeded)
        {
            return controller.BadRequest(result);
        }

        return controller.Ok(result);
    }
}
```

## **STEP 3 - Finishing the API Controller**

### **Base Api Controller** Will be used to inject Mediatr into all other Controllers

![Api Controller!](Assets/2020-11-20-11-16-51.png)

ApiController.cs

```cs
namespace Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
    private IMediator mediator;

    protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();
}
```

Now let's modify the Customer Controller to use Mediatr.

Inherit from the ApiController instead of ControllerBase

```cs
public class StockController : ApiController
```

Modify all the functions to use Mediatr and the new DataDTO's

```cs
namespace Api.Controllers;

using System.Threading.Tasks;
using Api.Helpers;
using Common.Entities;
using Common.Models;
using Core.Pizza.Commands;
using Core.Pizza.Queries;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class PizzaController : ApiController
{
	/// <summary>
	/// Get Pizza by Id.
	/// </summary>
	/// <param name="id">int.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Get a pizza</response>
	/// <response code="400">Error getting a pizza</response>
	/// <response code="404">Pizza not found</response>
	[HttpGet("{id}")]
	[ProducesResponseType(typeof(Result<PizzaModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	[ProducesResponseType(typeof(ErrorResult), 404)]
	public async Task<ActionResult> GetPizza(int id)
	{
		var result = await this.Mediator.Send(new GetPizzaQuery { Id = id });
		return ResponseHelper.ResponseOutcome(result, this);
	}

	/// <summary>
	/// Get all Pizzas.
	/// </summary>
	/// <returns>A <see cref="Task"/> repres
	/// enting the asynchronous operation.</returns>
	/// <response code="200">Pizza Search</response>
	/// <response code="400">Error searching for pizzas</response>
	[HttpPost]
	[ProducesResponseType(typeof(ListResult<PizzaModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	[Route("Search")]
	public async Task<ActionResult> Search()
	{
		var result = await this.Mediator.Send(new GetPizzasQuery());
		return ResponseHelper.ResponseOutcome(result, this);
	}

	/// <summary>
	/// Create Pizza.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     POST api/Pizza
	///     {
	///       "name": "Hawaiian",
	///       "description": "Hawaiian pizza is a pizza originating in Canada, and is traditionally topped with pineapple, tomato sauce, cheese, and either ham or bacon.",
	///       "price": "99"
	///     }
	/// </remarks>
	/// <param name="pizza">PizzaModel.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Pizza created</response>
	/// <response code="400">Error creating a pizza</response>
	[HttpPost]
	[ProducesResponseType(typeof(Result<PizzaModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	public async Task<ActionResult<PizzaModel>> Create(CreatePizzaModel pizza)
	{
		var result = await this.Mediator.Send(new CreatePizzaCommand
		{
			Data = pizza
		});

		return ResponseHelper.ResponseOutcome(result, this);
	}


	/// <summary>
	/// Update Pizza.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     PUT api/Pizza/1
	///     {
	///       "price": "119"
	///     }
	/// </remarks>
	/// <param name="pizza">PizzaModel.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Pizza updated</response>
	/// <response code="400">Error updating a pizza</response>
	/// <response code="404">Pizza not found</response>
	[HttpPut]
	[ProducesResponseType(typeof(Result<PizzaModel>), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	[ProducesResponseType(typeof(Result), 404)]
	public async Task<ActionResult> Update(UpdatePizzaModel pizza)
	{
		var result = await this.Mediator.Send(new UpdatePizzaCommand
		{
			Data = pizza
		});

		return ResponseHelper.ResponseOutcome(result, this);
	}

	/// <summary>
	/// Delete Pizza by Id.
	/// </summary>
	/// <param name="id">int.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Pizza deleted</response>
	/// <response code="400">Error deleting a pizza</response>
	[HttpDelete("{id}")]
	[ProducesResponseType(typeof(Result), 200)]
	[ProducesResponseType(typeof(ErrorResult), 400)]
	public async Task<ActionResult> Delete(int id)
	{
		var result = await this.Mediator.Send(new DeletePizzaCommand { Id = id });
		return ResponseHelper.ResponseOutcome(result, this);
	}
}
```

Complete the Customer Controller

![](./Assets/2023-04-13-05-44-28.png)

Right-Click on you Api project -> Properties -> Debug.

Change Launch Browser to Open "swagger"

![](Assets/2020-11-25-00-39-15.png)

Startup.cs should look like this when you are done.

```cs
namespace Api;

using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Core;
using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

public class Startup
{
	public IConfiguration configRoot
	{
		get;
	}

	public Startup(IConfiguration configuration) => configRoot = configuration;

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
			.AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new DefaultContractResolver())
			.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

		DependencyInjection.AddApplication(services);

		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "Pezza API",
				Version = "v1"
			});

			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			c.IncludeXmlComments(xmlPath);
		});

		services.AddDbContext<DatabaseContext>(options =>
			options.UseInMemoryDatabase(Guid.NewGuid().ToString())
		);
	}

	public void Configure(WebApplication app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		// Enable middleware to serve generated Swagger as a JSON endpoint.
		app.UseSwagger();
		//// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
		//// specifying the Swagger JSON endpoint.
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pezza API V1"));

		app.UseHttpsRedirection();


		app.UseRouting();

		app.UseEndpoints(endpoints => endpoints.MapControllers());

		app.UseAuthorization();

		app.Run();
	}
}
```

Press F5 and Run your API. You should see something like this.

![](./Assets/2023-04-13-05-45-44.png)

### You are done with the Back-End that will be used to build most of the Front-End stack.

## **Move to Phase 3**

[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%203)