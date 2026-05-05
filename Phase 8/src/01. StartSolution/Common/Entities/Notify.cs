namespace Common.Entities;

public class Notify
{
	public int Id { get; set; }

	public required int CustomerId { get; set; }

	public required string CustomerEmail { get; set; }

	public required string EmailContent { get; set; }

	public required bool Sent { get; set; }

	public DateTime? DateSent { get; set; }

	public virtual Customer Customer { get; set; }
}
