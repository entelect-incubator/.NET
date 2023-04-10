namespace Test.Core;

using System.Linq;
using System.Threading.Tasks;
using Bogus;
using NUnit.Framework;
using Common.DTO;
using Core;
using Test.Setup;
using Test.Setup.TestData.Stock;

[TestFixture]
public class TestPizzaCore : QueryTestBase
{
	private PizzaCore handler;

	private StockDTO pizza;

	[SetUp]
	public async Task Init()
	{
		this.handler = new PizzaCore(this.Context, Mapper());
		this.pizza = PizzaTestData.StockDTO;
		this.pizza = await this.handler.SaveAsync(this.pizza);
	}

	[Test]
	public async Task GetAsync()
	{
		var response = await this.handler.GetAsync(this.pizza.Id);
		Assert.IsTrue(response != null);
	}

	[Test]
	public async Task GetAllAsync()
	{
		var response = await this.handler.GetAllAsync();
		Assert.IsTrue(response.Count() == 1);
	}

	[Test]
	public void SaveAsync()
	{
		var outcome = this.pizza.Id != 0;
		Assert.IsTrue(outcome);
	}

	[Test]
	public async Task UpdateAsync()
	{
		var originalStock = this.pizza;
		this.pizza.Name = new Faker().Commerce.Product();
		var response = await this.handler.UpdateAsync(this.pizza);
		var outcome = response.Name.Equals(originalStock.Name);

		Assert.IsTrue(outcome);
	}

	[Test]
	public async Task DeleteAsync()
	{
		var response = await this.handler.DeleteAsync(this.pizza.Id);
		Assert.IsTrue(response);
	}
}
