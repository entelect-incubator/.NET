namespace Common.Models;

public sealed class CreatePizzaModel
{
	public string Name { get; set; }

	public string Description { get; set; }

	public decimal Price { get; set; }
}
