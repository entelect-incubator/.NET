namespace Common.Models.Order;

public class OrderModel
{
	public OrderModel() => this.Pizzas = new List<PizzaModel>();

	public int Id { get; set; }

	public required int CustomerId { get; set; }

	public CustomerModel Customer { get; set; }

	public List<int> PizzaIds { get; set; } // List of Pizza IDs

	public List<PizzaModel> Pizzas { get; set; }

	public DateTime? DateCreated { get; set; }

	public required bool Completed { get; set; }
}
