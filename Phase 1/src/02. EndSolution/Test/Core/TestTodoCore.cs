namespace Test.Core;

using Test.Setup.TestData.Pizza;

[TestFixture]
public class TestTodoCore : QueryTestBase
{
	private TodoCore handler;

	private TodoModel model;

	[OneTimeSetUp]
	public async Task Init()
	{
		this.handler = new TodoCore(this.Context);
		this.model = await this.handler.AddAsync(TodoTestData.TodoModel);
	}

	[Test]
	public async Task GetAllAsync()
	{
		var response = await this.handler.GetAllAsync(this.model.SessionId);
		Assert.That(response.Count(), Is.EqualTo(1));
	}

	[Test]
	public void AddAsync()
	{
		var outcome = this.model.Id != 0;
		Assert.That(outcome, Is.True);
	}

	[Test]
	public async Task CompleteAsync()
	{
		var response = await this.handler.CompleteAsync(this.model.Id);

		Assert.That(response, Is.True);
	}

	[Test]
	public async Task UpdateAsync()
	{
		var originalPizza = this.model;
		this.model.Task = new Faker().Random.Word();
		var response = await this.handler.UpdateAsync(this.model);
		var outcome = response.Task.Equals(originalPizza.Task);

		Assert.That(outcome, Is.True);
	}

	[Test]
	public async Task DeleteAsync()
	{
		var response = await this.handler.DeleteAsync(this.model.Id);
		Assert.That(response, Is.True);

		this.model = await this.handler.AddAsync(TodoTestData.TodoModel);
	}
}
