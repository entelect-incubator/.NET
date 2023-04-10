namespace Api.Controllers;

using System.Threading.Tasks;
using Common.Entities;
using Common.Models;
using Core.Contracts;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
	private readonly IPizzaCore PizzaCore;

	public PizzaController(IPizzaCore PizzaCore) => this.PizzaCore = PizzaCore;

	/// <summary>
	/// Get Pizza by Id.
	/// </summary>
	/// <param name="id">Pizza Id</param>
	/// <returns>ActionResult</returns>
	[HttpGet("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	public async Task<ActionResult> Get(int id)
	{
		var search = await this.PizzaCore.GetAsync(id);

		return (search == null) ? this.NotFound() : this.Ok(search);
	}

	/// <summary>
	/// Get all Pizzas.
	/// </summary>
	/// <returns>ActionResult</returns>
	[HttpPost("Search")]
	[ProducesResponseType(200)]
	public async Task<ActionResult> Search()
		=> this.Ok(await this.PizzaCore.GetAllAsync());

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
	/// <param name="model">Pizza Model</param>
	/// <returns>ActionResult</returns>
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult<Pizza>> Create([FromBody] PizzaModel model)
	{
		var result = await this.PizzaCore.SaveAsync(model);

		return (result == null) ? this.BadRequest() : this.Ok(result);
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
	/// <param name="model">Pizza Model</param>
	/// <returns>ActionResult</returns>
	[HttpPut]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Update([FromBody] PizzaModel model)
	{
		var result = await this.PizzaCore.UpdateAsync(model);

		return (result == null) ? this.BadRequest() : this.Ok(result);
	}

	/// <summary>
	/// Delete Pizza by Id.
	/// </summary>
	/// <param name="id">Pizza Id</param>
	/// <returns>ActionResult</returns>
	[HttpDelete("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Delete(int id)
	{
		var result = await this.PizzaCore.DeleteAsync(id);

		return (!result) ? this.BadRequest() : this.Ok(result);
	}
}
