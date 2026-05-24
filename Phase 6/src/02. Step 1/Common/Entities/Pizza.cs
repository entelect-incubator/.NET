namespace Common.Entities;

public sealed class Pizza
{
	public int Id { get; set; }

	public required string Name { get; set; }

	public string? Description { get; set; }

	public decimal? Price { get; set; }

	public DateTime DateCreated { get; set; }
}
