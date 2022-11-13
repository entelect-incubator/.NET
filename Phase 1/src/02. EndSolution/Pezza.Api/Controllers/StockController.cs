namespace Pezza.Api.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pezza.Common.DTO;
using Pezza.Common.Entities;
using Pezza.Core.Contracts;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
	private readonly IStockCore stockCore;

	public StockController(IStockCore stockCore) => this.stockCore = stockCore;

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
		var search = await this.stockCore.GetAsync(id);

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
		var result = await this.stockCore.GetAllAsync();

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
		var result = await this.stockCore.SaveAsync(dto);

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
		var result = await this.stockCore.UpdateAsync(dto);

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
		var result = await this.stockCore.DeleteAsync(id);

		return (!result) ? this.BadRequest() : this.Ok(result);
	}
}
