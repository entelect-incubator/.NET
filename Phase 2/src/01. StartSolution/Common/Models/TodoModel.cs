namespace Common.Models;

public sealed class TodoModel
{
	public int Id { get; set; }

	public required string Task { get; set; }

	public bool IsCompleted { get; set; }

	public DateTime? DateCreated { get; set; }

	public Guid SessionId { get; set; }
}
