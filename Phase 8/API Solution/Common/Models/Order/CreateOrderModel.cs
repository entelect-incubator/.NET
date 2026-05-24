namespace Common.Models.Order;

public sealed class CreateOrderModel
{
	public required int CustomerId { get; set; }

	public required List<int> PizzaIds { get; set; }
}
