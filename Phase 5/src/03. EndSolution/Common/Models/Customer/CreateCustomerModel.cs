namespace Common.Models.Customer;

public sealed class CreateCustomerModel
{
	public required string Name { get; set; }

	public required string Address { get; set; }

	public string? Email { get; set; }

	public string? Cellphone { get; set; }
}
