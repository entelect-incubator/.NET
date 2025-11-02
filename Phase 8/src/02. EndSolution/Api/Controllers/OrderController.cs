namespace Api.Controllers;

using Common.Models.Order;
using Core.Order.Commands;

[ApiController]
[Route("[controller]")]
public class OrderController() : ApiController
{
	/// <summary>
	/// Order Pizza.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     POST /Order
	///     {
	///       "customerId": 1,
	///       "pizzaIds": [1, 2, 3, 4, 5]
	///     }
	/// </remarks>
	/// <param name="model">Create Order Model</param>
	/// <returns>ActionResult</returns>
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult<OrderModel>> Create([FromBody] OrderModel model)
	{
		var result = await this.Mediator.Send(new OrderCommand
		{
			Data = model
		});

		return ResponseHelper.ResponseOutcome(result, this);
	}
}
