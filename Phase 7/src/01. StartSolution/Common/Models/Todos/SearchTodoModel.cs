namespace Common.Models.Todos;

public sealed class SearchTodoModel
{
	public string? Task { get; set; }

	public bool? IsCompleted { get; set; }

	public DateTime? DateCreated { get; set; }
	
	public int? Year { get; set; }

	public int? Month { get; set; }

	public int? Day { get; set; }

	public Guid? SessionId { get; set; }

	public string? OrderBy { get; set; }

	public PagingArgs PagingArgs { get; set; } = PagingArgs.NoPaging;
}
