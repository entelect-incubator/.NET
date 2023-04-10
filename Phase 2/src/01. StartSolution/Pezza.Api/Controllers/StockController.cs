namespace Api.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.DTO;
using Common.Entities;
using Core.Contracts;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
	private readonly IPizzaCore pizzaCore;

	public StockController(IPizzaCore pizzaCore) => this.pizzaCore = pizzaCore;

	/// <summary>
	/// Get Stock by Id.
	/// </summary>
	/// <param name="id">Stock Id</param>
	/// <returns>ActionResult</returns>
	[HttpGet("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	public async Task<ActionResult> Get(int id)
	{
		var search = await this.pizzaCore.GetAsync(id);

		return (search == null) ? this.NotFound() : this.Ok(search);
	}

	/// <summary>
	/// Get all Stock.
	/// </summary>
	/// <returns>ActionResult</returns>
	[HttpPost("Search")]
	[ProducesResponseType(200)]
	public async Task<ActionResult> Search()
	{
		var result = await this.pizzaCore.GetAllAsync();

		return this.Ok(result);
	}

	/// <summary>
	/// Create Stock.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     POST api/Stock
	///     {
	///       "name": "Tomatoes",
	///       "UnitOfMeasure": "Kg",
	///       "ValueOfMeasure": "1",
	///       "Quantity": "50"
	///     }
	/// </remarks>
	/// <param name="dto">Stock Model</param>
	/// <returns>ActionResult</returns>
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult<Stock>> Create([FromBody] StockDTO dto)
	{
		var result = await this.pizzaCore.SaveAsync(dto);

		return (result == null) ? this.BadRequest() : this.Ok(result);
	}

	/// <summary>
	/// Update Stock.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     PUT api/Stock/1
	///     {
	///       "Quantity": "30"
	///     }
	/// </remarks>
	/// <param name="dto">Stock Model</param>
	/// <returns>ActionResult</returns>
	[HttpPut]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Update([FromBody] StockDTO dto)
	{
		var result = await this.pizzaCore.UpdateAsync(dto);

		return (result == null) ? this.BadRequest() : this.Ok(result);
	}

	/// <summary>
	/// Remove Stock by Id.
	/// </summary>
	/// <param name="id">Stock Id</param>
	/// <returns>ActionResult</returns>
	[HttpDelete("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Delete(int id)
	{
		var result = await this.pizzaCore.DeleteAsync(id);

		return (!result) ? this.BadRequest() : this.Ok(result);
	}
}
