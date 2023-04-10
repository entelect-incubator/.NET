namespace Test.Setup.TestData.Pizza;

using Bogus;
using Common.Entities;
using Common.Models;

public static class PizzaTestData
{
	public static Faker faker = new();

	public static Pizza Pizza = new()
	{
		Id = 1,
		Name = faker.PickRandom(new List<string>()
		{
			"Veggie Pizza",
			"Pepperoni Pizza",
			"Meat Pizza",
			"Margherita Pizza",
			"BBQ Chicken Pizza",
			"Hawaiian Pizza"
		}),
		Description = " ",
		Price = faker.Finance.Amount(),
		DateCreated = DateTime.Now,
	};

	public static PizzaModel PizzaModel = new()
	{
		Id = 1,
		Name = faker.PickRandom(new List<string>()
		{
			"Veggie Pizza",
			"Pepperoni Pizza",
			"Meat Pizza",
			"Margherita Pizza",
			"BBQ Chicken Pizza",
			"Hawaiian Pizza"
		}),
		Description = " ",
		Price = faker.Finance.Amount(),
		DateCreated = DateTime.Now	
	};
}