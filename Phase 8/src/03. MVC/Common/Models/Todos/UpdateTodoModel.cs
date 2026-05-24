namespace Common.Models.Todos;

public sealed class UpdateTodoModel
{
	public string? Task { get; set; }

	public bool? IsCompleted { get; set; }

	public Guid? SessionId { get; set; }
}
