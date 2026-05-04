namespace Common.Models.Pizza;

public sealed class SearchPizzaModel
{
	public string? Name { get; set; }

	public string? Description { get; set; }

	public decimal? Price { get; set; }

	public DateTime? DateCreated { get; set; }
}
