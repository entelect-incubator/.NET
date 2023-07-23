namespace Common.Models;

public sealed class CreatePizzaModel
{
	public required string Name { get; set; }

	public string? Description { get; set; }

	public required decimal Price { get; set; }
}
