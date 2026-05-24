namespace Common.Models.Order;

public class OrderModel
{
	public required CustomerModel Customer { get; set; }

	public required List<PizzaModel> Pizzas { get; set; }
}
