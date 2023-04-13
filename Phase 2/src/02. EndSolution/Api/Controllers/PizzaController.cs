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
