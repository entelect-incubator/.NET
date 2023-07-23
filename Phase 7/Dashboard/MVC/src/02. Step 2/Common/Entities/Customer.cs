namespace Common.Entities;

public sealed class Customer
{
	public Customer() => this.Notifies = new HashSet<Notify>();

	public int Id { get; set; }

	public required string Name { get; set; }

	public string? Address { get; set; }

	public string? Email { get; set; }

	public string? Cellphone { get; set; }

	public DateTime DateCreated { get; set; }

	public ICollection<Notify> Notifies { get; }
}
