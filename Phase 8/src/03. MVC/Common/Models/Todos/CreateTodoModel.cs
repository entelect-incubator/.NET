namespace Common.Models.Todos;

public sealed class CreateTodoModel
{
	public required string Task { get; set; }

	public bool IsCompleted { get; set; }

	public Guid SessionId { get; set; }
}
