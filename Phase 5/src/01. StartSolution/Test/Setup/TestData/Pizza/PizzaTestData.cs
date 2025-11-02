namespace Test.Setup.TestData.Pizza;

public static class PizzaTestData
{
	/// <summary>
	/// Standard Pezza pizza menu - aligned with Theme project
	/// Source: Theme/index.html, SOLUTION.md
	/// </summary>
	private static readonly List<string> PizzaNames =
	[
		"Hawaiian Pizza",
		"Pepperoni Pizza",
		"Regina Pizza",
		"Margherita Pizza"
	];

	private static readonly Faker Faker = new();

	public static PizzaModel Pizza => new()
	{
		Id = 1,
		Name = Faker.PickRandom(PizzaNames),
		Description = string.Empty,
		Price = Faker.Finance.Amount(),
		DateCreated = DateTime.Now,
	};

	public static PizzaModel PizzaModel => new()
	{
		Id = 1,
		Name = Faker.PickRandom(PizzaNames),
		Description = string.Empty,
		Price = Faker.Finance.Amount(),
		DateCreated = DateTime.Now
	};
}
