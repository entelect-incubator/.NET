namespace Test.Core;

using Common.Models;
using global::Core.Pizza.Commands;
using global::Core.Pizza.Queries;
using Test.Setup.TestData.Pizza;

[TestFixture]
public class TestPizzaCore : QueryTestBase
{
	private PizzaModel model;

	[SetUp]
	public async Task Init()
	{
		this.model = PizzaTestData.PizzaModel;
		var sutCreate = new CreatePizzaCommand(this.Context);
		var resultCreate = await sutCreate.ExecuteAsync(
			new CreatePizzaModel
			{
				Name = this.model.Name,
				Price = 19
			}, CancellationToken.None);

		if (resultCreate.HasError)
		{
			Assert.That(false, Is.True);
		}

		this.model = resultCreate.Data;
	}

	[Test]
	public async Task GetAsync()
	{
		var sutGet = new GetPizzaQuery(this.Context);
		var resultGet = await sutGet.ExecuteAsync(this.model.Id, CancellationToken.None);

		Assert.That(resultGet?.Data, Is.Not.Null);
	}

	[Test]
	public async Task GetAllAsync()
	{
		var sutGetAll = new GetPizzasQuery(this.Context);
		var resultGetAll = await sutGetAll.ExecuteAsync(CancellationToken.None);

		Assert.That(resultGetAll?.Count, Is.Not.Null);
		Assert.That(resultGetAll?.Count, Is.GreaterThanOrEqualTo(1));
	}

	[Test]
	public void SaveAsync() => Assert.That(this.model, Is.Not.Null);

	[Test]
	public async Task UpdateAsync()
	{
		var sutUpdate = new UpdatePizzaCommand(this.Context);
		var resultUpdate = await sutUpdate.ExecuteAsync(this.model.Id,
			new UpdatePizzaModel
			{
				Price = 20
			}, CancellationToken.None);

		Assert.That(resultUpdate.HasError, Is.False);
	}

	[Test]
	public async Task DeleteAsync()
	{
		var sutDelete = new DeletePizzaCommand(this.Context);
		var outcomeDelete = await sutDelete.ExecuteAsync(this.model.Id, CancellationToken.None);

		Assert.That(outcomeDelete.HasError, Is.False);
	}
}
