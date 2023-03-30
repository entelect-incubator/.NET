namespace Pezza.Test.Setup.TestData.Stock;

using System;
using Bogus;
using Pezza.Common.Entities;
using Pezza.Common.Models;

public static class PizzaTestData
{
	public static Faker faker = new Faker();

	public static Pizza Pizza = new Pizza()
	{
		Id = 1,
		Name = faker.PickRandom(pizzas),
		Description = faker.Lorem.Sentence(),
		Price = faker.Finance.Amount(),
		DateCreated = DateTime.Now,
	};

	public static PizzaModel PizzaModel = new PizzaModel()
	{
		Id = 1,
		Name = faker.PickRandom(pizzas),
		Description = faker.Lorem.Sentence(),
		Price = faker.Finance.Amount(),
		DateCreated = DateTime.Now,
		PictureUrl 
		
	};

	private static readonly List<PizzaTestDataModel> pizzas = new() 
	{ 
		new PizzaTestDataModel {"Veggie Pizza",
		"Pepperoni Pizza",
		"Meat Pizza",
		"Margherita Pizza",
		"BBQ Chicken Pizza",
		"Hawaiian Pizza"
	};
}
