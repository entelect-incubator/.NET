namespace Common.Models;

public sealed class PizzaModel
{
	public required int Id { get; set; }

	public required string Name { get; set; }

	public string? Description { get; set; }

	public decimal? Price { get; set; }

	public DateTime? DateCreated { get; set; }
}
