namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController(IPizzaCore pizzaCore) : ControllerBase
{
	/// <summary>
	/// Get Pizza by Id.
	/// </summary>
	/// <param name="id">Pizza Id</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>ActionResult with Pizza model or NotFound</returns>
	[HttpGet("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	public async Task<ActionResult> Get(int id, CancellationToken cancellationToken = default)
	{
		var search = await pizzaCore.GetAsync(id);
		return search == null ? NotFound() : Ok(search);
	}

	/// <summary>
	/// Get all Pizzas.
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>ActionResult with list of pizzas</returns>
	[HttpPost("Search")]
	[ProducesResponseType(200)]
	public async Task<ActionResult> Search(CancellationToken cancellationToken = default)
		=> Ok(await pizzaCore.GetAllAsync());

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
	/// <param name="model">Pizza model to create</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>ActionResult with created pizza or BadRequest</returns>
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult<Pizza>> Create([FromBody] PizzaModel model, CancellationToken cancellationToken = default)
	{
		var result = await pizzaCore.SaveAsync(model);
		return result == null ? BadRequest() : Ok(result);
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
	/// <param name="model">Pizza model with updates</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>ActionResult with updated pizza or BadRequest</returns>
	[HttpPut]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Update([FromBody] PizzaModel model, CancellationToken cancellationToken = default)
	{
		var result = await pizzaCore.UpdateAsync(model);
		return result == null ? BadRequest() : Ok(result);
	}

	/// <summary>
	/// Delete Pizza by Id.
	/// </summary>
	/// <param name="id">Pizza Id to delete</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>ActionResult with success or BadRequest</returns>
	[HttpDelete("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken = default)
	{
		var result = await pizzaCore.DeleteAsync(id);
		return !result ? BadRequest() : Ok(result);
	}
}
