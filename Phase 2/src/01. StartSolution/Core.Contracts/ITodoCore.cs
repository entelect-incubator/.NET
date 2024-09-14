namespace Core.Contracts;

public interface ITodoCore
{
	Task<IEnumerable<TodoModel>?> GetAllAsync(Guid sessionId, CancellationToken cancellationToken = default);

	Task<TodoModel?> AddAsync(TodoModel model, CancellationToken cancellationToken = default);

	Task<bool> CompleteAsync(int id, CancellationToken cancellationToken = default);

	Task<TodoModel?> UpdateAsync(TodoModel model, CancellationToken cancellationToken = default);

	Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
