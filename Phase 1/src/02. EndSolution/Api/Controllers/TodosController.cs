namespace Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TodosController(ITodoCore core) : ControllerBase
{
	/// <summary>
	/// Get all Todos.
	/// </summary>
	/// <returns>ActionResult</returns>
	[HttpPost("Search")]
	[ProducesResponseType(200)]
	public async Task<ActionResult> Search(Guid sessionId, CancellationToken cancellationToken = default)
		=> this.Ok(await core.GetAllAsync(sessionId, cancellationToken));

	/// <summary>
	/// Create a task.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     POST api/Todo
	///     {
	///       "task": "New task",
	///     }
	/// </remarks>
	/// <param name="model">Todo Model</param>
	/// <param name="cancellationToken">Cancellation Token</param>
	/// <returns>ActionResult</returns>
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult<Todo>> Add([FromBody] TodoModel model, CancellationToken cancellationToken = default)
	{
		var result = await core.AddAsync(model, cancellationToken);

		return (result is null) ? this.BadRequest() : this.Ok(result);
	}

	/// <summary>
	/// Complete a task.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     PUT api/Todo/Complete
	///     {
	///       "id": "1"
	///     }
	/// </remarks>
	/// <param name="id">Task id</param>
	/// <param name="cancellationToken">Cancellation Token</param>
	/// <returns>ActionResult</returns>
	[HttpPost("Complete")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Complete([FromBody] int id, CancellationToken cancellationToken = default)
	{
		var result = await core.CompleteAsync(id, cancellationToken);

		return (result is false) ? this.BadRequest() : this.Ok(result);
	}


	/// <summary>
	/// Update Todo.
	/// </summary>
	/// <remarks>
	/// Sample request:
	///
	///     PUT api/Todo/1
	///     {
	///       "Task": "New task"
	///     }
	/// </remarks>
	/// <param name="model">Todo Model</param>
	/// <param name="cancellationToken">Cancellation Token</param>
	/// <returns>ActionResult</returns>
	[HttpPut]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Update([FromBody] TodoModel model, CancellationToken cancellationToken = default)
	{
		var result = await core.UpdateAsync(model, cancellationToken);

		return (result is null) ? this.BadRequest() : this.Ok(result);
	}

	/// <summary>
	/// Delete a task by Id.
	/// </summary>
	/// <param name="id">Task Id</param>
	/// <param name="cancellationToken">Cancellation Token</param>
	/// <returns>ActionResult</returns>
	[HttpDelete("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken = default)
	{
		var result = await core.DeleteAsync(id, cancellationToken);

		return (!result) ? this.BadRequest() : this.Ok(result);
	}
}
