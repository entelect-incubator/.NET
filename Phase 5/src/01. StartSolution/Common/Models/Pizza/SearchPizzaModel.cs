namespace Common.Models;

public sealed class SearchPizzaModel
{
	public string? Name { get; set; }

	public string? Description { get; set; }

	public string? OrderBy { get; set; }

	public PagingArgs PagingArgs { get; set; } = new();
}
