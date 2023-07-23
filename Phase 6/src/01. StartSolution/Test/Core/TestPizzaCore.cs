namespace Test.Core;

using Common.Models;
using global::Core.Pizza.Commands;
using global::Core.Pizza.Queries;
using Test.Setup.TestData.Pizza;
using static global::Core.Pizza.Commands.CreatePizzaCommand;
using static global::Core.Pizza.Commands.DeletePizzaCommand;
using static global::Core.Pizza.Commands.UpdatePizzaCommand;
using static global::Core.Pizza.Queries.GetPizzaQuery;
using static global::Core.Pizza.Queries.GetPizzasQuery;

[TestFixture]
public class TestPizzaCore : QueryTestBase
{
	private PizzaModel model;

	[SetUp]
	public async Task Init()
	{
		this.model = PizzaTestData.PizzaModel;
		var sutCreate = new CreatePizzaCommandHandler(this.Context, this.CachingService);
		var resultCreate = await sutCreate.Handle(
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
			Assert.IsTrue(false);
		}

		this.model = resultCreate.Data;
	}

	[Test]
	public async Task GetAsync()
	{
		var sutGet = new GetPizzaQueryHandler(this.Context);
		var resultGet = await sutGet.Handle(
			new GetPizzaQuery
			{
				Id = this.model.Id
			}, CancellationToken.None);

		Assert.IsTrue(resultGet?.Data != null);
	}

	[Test]
	public async Task GetAllAsync()
	{
		var sutGetAll = new GetPizzasQueryHandler(this.Context, this.CachingService);
		var resultGetAll = await sutGetAll.Handle(new GetPizzasQuery(), CancellationToken.None);

		Assert.IsTrue(resultGetAll?.Data.Count == 1);
	}

	[Test]
	public void SaveAsync() => Assert.IsTrue(this.model != null);

	[Test]
	public async Task UpdateAsync()
	{
		var sutUpdate = new UpdatePizzaCommandHandler(this.Context, this.CachingService);
		var resultUpdate = await sutUpdate.Handle(
			new UpdatePizzaCommand
			{
				Id = this.model.Id,
				Data = new UpdatePizzaModel
				{
					Price = 20
				}
			}, CancellationToken.None);

		Assert.IsTrue(resultUpdate.Succeeded);
	}

	[Test]
	public async Task DeleteAsync()
	{
		var sutDelete = new DeletePizzaCommandHandler(this.Context, this.CachingService);
		var outcomeDelete = await sutDelete.Handle(
			new DeletePizzaCommand
			{
				Id = this.model.Id
			}, CancellationToken.None);

		Assert.IsTrue(outcomeDelete.Succeeded);
	}
}
