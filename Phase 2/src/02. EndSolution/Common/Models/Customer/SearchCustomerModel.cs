namespace Common.Models.Customer;

public sealed class SearchCustomerModel
{
	public string? Name { get; set; }

	public string? Address { get; set; }

	public string? Email { get; set; }

	public string? Cellphone { get; set; }

	public DateTime? DateCreated { get; set; }
}
