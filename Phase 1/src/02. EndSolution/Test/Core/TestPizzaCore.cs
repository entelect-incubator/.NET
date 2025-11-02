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
		Assert.That(response, Is.Not.Null);
		Assert.That(response.Name, Is.EqualTo(this.Pizza.Name));
	}

	[Test]
	public async Task GetAllAsync()
	{
		var response = await this.handler.GetAllAsync();
		Assert.That(response.Count(), Is.EqualTo(1));
	}

	[Test]
	public async Task SaveAsync()
	{
		var newPizza = new PizzaModel
		{
			Id = 0,
			Name = new Faker().Commerce.Product(),
			Description = "Test Pizza",
			Price = 9.99m,
			DateCreated = DateTime.UtcNow
		};
		var response = await this.handler.SaveAsync(newPizza);

		Assert.That(response, Is.Not.Null);
		Assert.That(response.Id, Is.Not.EqualTo(0));
	}

	[Test]
	public async Task UpdateAsync()
	{
		var originalName = this.Pizza.Name;
		this.Pizza.Name = new Faker().Commerce.Product();
		var response = await this.handler.UpdateAsync(this.Pizza);

		Assert.That(response, Is.Not.Null);
		Assert.That(response.Name, Is.Not.EqualTo(originalName));
		Assert.That(response.Name, Is.EqualTo(this.Pizza.Name));
	}

	[Test]
	public async Task DeleteAsync()
	{
		var response = await this.handler.DeleteAsync(this.Pizza.Id);
		Assert.That(response, Is.True);
	}
}
