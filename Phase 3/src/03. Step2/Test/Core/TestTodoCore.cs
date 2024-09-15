namespace Test.Core;

using Common.Models.Todos;
using global::Core.Todos.Commands;
using global::Core.Todos.Queries;
using Test.Setup.TestData.Pizza;

[TestFixture]
public class TestTodoCore : QueryTestBase
{
	private TodoModel model;

	[OneTimeSetUp]
	public async Task Init()
	{
		var sutCreate = new AddTodoCommandHandler(this.Context);
		var resultCreate = await sutCreate.Handle(
			new AddTodoCommand
			{
				Data = TodoTestData.CreateTodoModel
			}, CancellationToken.None);

		if (!resultCreate.Succeeded)
		{
			Assert.Fail();
		}

		this.model = resultCreate.Data;
	}

	[Test, Order(1)]
	public void AddAsync()
	{
		var outcome = this.model.Id != 0;
		Assert.That(outcome, Is.True);
	}

	[Test, Order(2)]
	public async Task GetAllAsync()
	{
		var sutGetAll = new GetTodosQueryHandler(this.Context);
		var resultGetAll = await sutGetAll.Handle(new GetTodosQuery()
		{
			Data = new SearchTodoModel()
			{
				SessionId = this.model.SessionId
			},
		}, CancellationToken.None);

		Assert.That(resultGetAll.Succeeded, Is.True);
		Assert.That(resultGetAll.Data.Count, Is.EqualTo(1));
	}

	[Test, Order(3)]
	public async Task CompleteAsync()
	{
		var sutUpdate = new CompleteTodoCommandHandler(this.Context);
		var resultUpdate = await sutUpdate.Handle(
			new CompleteTodoCommand
			{
				Id = this.model.Id,
			}, CancellationToken.None);

		Assert.That(resultUpdate.Succeeded, Is.True);
		Assert.That(resultUpdate.Data?.IsCompleted, Is.True);
	}

	[Test, Order(4)]
	public async Task UpdateAsync()
	{
		var sutUpdate = new UpdateTodoCommandHandler(this.Context);
		var resultUpdate = await sutUpdate.Handle(
			new UpdateTodoCommand
			{
				Id = this.model.Id,
				Data = TodoTestData.UpdateTodoModel
			}, CancellationToken.None);

		Assert.That(resultUpdate.Succeeded, Is.True);
	}

	[Test, Order(5)]
	public async Task DeleteAsync()
	{
		var sutDelete = new DeleteTodoCommandHandler(this.Context);
		var resultDelete = await sutDelete.Handle(
			new DeleteTodoCommand
			{
				Id = this.model.Id,
			}, CancellationToken.None);

		Assert.That(resultDelete.Succeeded, Is.True);
	}
}
