namespace Api.Controllers;

using Api.Helpers;
using Common.Models.Todos;
using Core.Todos.Commands;
using Core.Todos.Queries;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TodosController : ApiController
{
	/// <summary>
	/// Get all Todos.
	/// </summary>
	/// <returns>ActionResult</returns>
	[HttpPost("Search")]
	[ProducesResponseType(200)]
	public async Task<ActionResult> Search(Guid sessionId, CancellationToken cancellationToken = default)
		=> ResponseHelper.ResponseOutcome(await this.Mediator.Send(new GetTodosQuery() { SessionId = sessionId }, cancellationToken), this);

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
	/// <param name="model">Create Todo Model</param>
	/// <param name="cancellationToken">Cancellation Token</param>
	/// <returns>ActionResult</returns>
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult<Todo>> Add([FromBody] CreateTodoModel model, CancellationToken cancellationToken = default)
		=> ResponseHelper.ResponseOutcome(await this.Mediator.Send(new AddTodoCommand() { Data = model }, cancellationToken), this);

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
		=> ResponseHelper.ResponseOutcome(await this.Mediator.Send(new CompleteTodoCommand() { Id = id }, cancellationToken), this);


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
	/// <param name="id">Todo id</param>
	/// <param name="model">Update Todo Model</param>
	/// <param name="cancellationToken">Cancellation Token</param>
	/// <returns>ActionResult</returns>
	[HttpPut("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public async Task<ActionResult> Update(int id, [FromBody] UpdateTodoModel model, CancellationToken cancellationToken = default)
		=> ResponseHelper.ResponseOutcome(await this.Mediator.Send(new UpdateTodoCommand() { Id = id, Data = model }, cancellationToken), this);

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
		=> ResponseHelper.ResponseOutcome(await this.Mediator.Send(new DeleteTodoCommand() { Id = id }, cancellationToken), this);
}
