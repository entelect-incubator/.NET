namespace Test.Core;

using Common.Models.Pizza;
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
		var sutCreate = new CreatePizzaCommandHandler(this.Context);
		var resultCreate = await sutCreate.HandleAsync(
			new CreatePizzaCommand
			{
				Data = new CreatePizzaModel
				{
					Name = this.model.Name,
					Price = 19
				}
			}, CancellationToken.None);

		if (!resultCreate.Succeeded)
		{
			Assert.That(false, Is.True);
		}

		this.model = resultCreate.Data;
	}

	[Test]
	public async Task GetAsync()
	{
		var sutGet = new GetPizzaQueryHandler(this.Context);
		var resultGet = await sutGet.HandleAsync(
			new GetPizzaQuery
			{
				Id = this.model.Id
			}, CancellationToken.None);

		Assert.That(resultGet?.Data, Is.Not.Null);
	}

	[Test]
	public async Task GetAllAsync()
	{
		var sutGetAll = new GetPizzasQueryHandler(this.Context);
		var resultGetAll = await sutGetAll.HandleAsync(new GetPizzasQuery(), CancellationToken.None);

		Assert.That(resultGetAll?.Count, Is.EqualTo(1));
	}

	[Test]
	public void SaveAsync() => Assert.That(this.model, Is.Not.Null);

	[Test]
	public async Task UpdateAsync()
	{
		var sutUpdate = new UpdatePizzaCommandHandler(this.Context);
		var resultUpdate = await sutUpdate.HandleAsync(
			new UpdatePizzaCommand
			{
				Id = this.model.Id,
				Data = new UpdatePizzaModel
				{
					Price = 20
				}
			}, CancellationToken.None);

		Assert.That(resultUpdate.Succeeded, Is.True);
	}

	[Test]
	public async Task DeleteAsync()
	{
		var sutDelete = new DeletePizzaCommandHandler(this.Context);
		var outcomeDelete = await sutDelete.HandleAsync(
			new DeletePizzaCommand
			{
				Id = this.model.Id
			}, CancellationToken.None);

		Assert.That(outcomeDelete.Succeeded, Is.True);
	}
}
