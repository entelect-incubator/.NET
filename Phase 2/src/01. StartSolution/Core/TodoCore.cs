namespace Core;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class TodoCore(DatabaseContext databaseContext) : ITodoCore
{
	public async Task<IEnumerable<TodoModel>?> GetAllAsync(Guid sessionId, CancellationToken cancellationToken = default)
		=> (await databaseContext.Todos.Where(x => x.SessionId == sessionId).Select(x => x).AsNoTracking().ToListAsync(cancellationToken)).Map();

	public async Task<TodoModel?> AddAsync(TodoModel pizza, CancellationToken cancellationToken = default)
	{
		if (pizza is null)
		{
			return null;
		}

		var entity = pizza.Map();
		entity.DateCreated = DateTime.UtcNow;
		databaseContext.Todos.Add(entity);
		var changeCount = await databaseContext.SaveChangesAsync(cancellationToken);
		if (changeCount is 0)
		{
			return null;
		}

		pizza.Id = entity.Id;
		return entity.Map();
	}

	public async Task<bool> CompleteAsync(int id, CancellationToken cancellationToken = default)
	{
		var findEntity = await databaseContext.Todos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
		if (findEntity is null)
		{
			return false;
		}

		findEntity.IsCompleted = true;
		databaseContext.Todos.Update(findEntity);
		var changeCount = await databaseContext.SaveChangesAsync(cancellationToken);
		return changeCount is 0 ? false : true;
	}

	public async Task<TodoModel?> UpdateAsync(TodoModel model, CancellationToken cancellationToken = default)
	{
		var findEntity = await databaseContext.Todos.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
		if (findEntity is null)
		{
			return null;
		}

		findEntity.Task = !string.IsNullOrEmpty(model.Task) ? model.Task : findEntity.Task;
		findEntity.IsCompleted = model.IsCompleted != findEntity.IsCompleted ? model.IsCompleted : findEntity.IsCompleted;
		await databaseContext.SaveChangesAsync(cancellationToken);

		return findEntity.Map();
	}

	public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
	{
		var todo = await databaseContext.Todos.FindAsync(id, cancellationToken);
		var result = 0;
		if (todo is not null)
		{
			databaseContext.Todos.Remove(todo);
			result = await databaseContext.SaveChangesAsync(cancellationToken);
		}

		return result == 1;
	}
}