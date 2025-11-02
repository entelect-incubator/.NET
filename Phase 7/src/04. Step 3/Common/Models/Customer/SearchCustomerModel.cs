namespace Common.Models;

public sealed class SearchCustomerModel
{
	public string? Name { get; set; }

	public string? Address { get; set; }

	public string? Email { get; set; }

	public string? Cellphone { get; set; }

	public DateTime? DateCreated { get; set; }

	public string? OrderBy { get; set; }

	public PagingArgs PagingArgs { get; set; } = PagingArgs.NoPaging;
}
