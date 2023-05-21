namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController(IPizzaCore pizzaCore) : ControllerBase
{
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
		var search = await pizzaCore.GetAsync(id);

		return (search == null) ? this.NotFound() : this.Ok(search);
	}

	/// <summary>
	/// Get all Pizzas.
	/// </summary>
	/// <returns>ActionResult</returns>
	[HttpPost("Search")]
	[ProducesResponseType(200)]
	public async Task<ActionResult> Search()
		=> this.Ok(await pizzaCore.GetAllAsync());

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
		var result = await pizzaCore.SaveAsync(model);

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
		var result = await pizzaCore.UpdateAsync(model);

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
		var result = await pizzaCore.DeleteAsync(id);

		return (!result) ? this.BadRequest() : this.Ok(result);
	}
}
