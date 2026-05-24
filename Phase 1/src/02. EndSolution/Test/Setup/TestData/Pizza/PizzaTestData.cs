namespace Test.Setup.TestData.Pizza;

public static class PizzaTestData
{
	/// <summary>
	/// Standard Pezza pizza menu - aligned with Theme project
	/// Source: Theme/index.html, SOLUTION.md
	/// </summary>
	private static readonly List<string> Pizzas = 
	[
		"Hawaiian Pizza",
		"Pepperoni Pizza",
		"Regina Pizza",
		"Margherita Pizza"
	];

	public static Faker Faker = new();

	public static PizzaModel PizzaModel => new()
	{
		Id = 0,
		Name = Faker.PickRandom(Pizzas),
		Description = "Test Pizza",
		Price = Faker.Finance.Amount(1, 20),
		DateCreated = DateTime.UtcNow
	};

	public static Common.Entities.Pizza PizzaEntity => new()
	{
		Id = 1,
		Name = Faker.PickRandom(Pizzas),
		Description = "Test Pizza",
		Price = Faker.Finance.Amount(1, 20),
		DateCreated = DateTime.UtcNow
	};
}
