namespace Test.Core;

using Test.Setup.TestData.Pizza;

[TestFixture]
public class TestPizzaCore : QueryTestBase
{
	private PizzaCore handler;

	private PizzaModel Pizza;

	[SetUp]
	public async Task Init()
	{
		this.handler = new PizzaCore(this.Context);
		this.Pizza = PizzaTestData.PizzaModel;
		this.Pizza = await this.handler.SaveAsync(this.Pizza);
	}

	[Test]
	public async Task GetAsync()
	{
		var response = await this.handler.GetAsync(this.Pizza.Id);
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
		var outcome = this.Pizza.Id != 0;
		Assert.IsTrue(outcome);
	}

	[Test]
	public async Task UpdateAsync()
	{
		var originalPizza = this.Pizza;
		this.Pizza.Name = new Faker().Commerce.Product();
		var response = await this.handler.UpdateAsync(this.Pizza);
		var outcome = response.Name.Equals(originalPizza.Name);

		Assert.IsTrue(outcome);
	}

	[Test]
	public async Task DeleteAsync()
	{
		var response = await this.handler.DeleteAsync(this.Pizza.Id);
		Assert.IsTrue(response);
	}
}
