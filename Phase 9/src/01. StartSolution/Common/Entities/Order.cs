namespace Common.Entities;

using System.Collections.ObjectModel;

public class Order
{
	public Order() => this.Pizzas = new HashSet<Pizza>();

	public int Id { get; set; }

	public required int CustomerId { get; set; }

	public DateTime? DateCreated { get; set; }

	public required bool Completed { get; set; }

////	public List<int> PizzaIds { get; set; } // List of Pizza IDs

	public virtual Customer Customer { get; set; } = null!;

	public ICollection<Pizza> Pizzas { get; set; }
}
