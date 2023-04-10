namespace Test.Setup.TestData.Stock;

using System;
using Bogus;
using Common.DTO;
using Common.Entities;

public static class PizzaTestData
{
	public static Faker faker = new Faker();

	public static Stock Stock = new Stock()
	{
		Comment = faker.Lorem.Sentence(),
		DateCreated = DateTime.Now,
		ExpiryDate = DateTime.Now.AddMonths(1),
		Name = faker.Commerce.Product(),
		Quantity = 1,
		UnitOfMeasure = "kg",
		ValueOfMeasure = 10.5
	};

	public static StockDTO StockDTO = new StockDTO()
	{
		Comment = faker.Lorem.Sentence(),
		ExpiryDate = DateTime.Now.AddMonths(1),
		Name = faker.Commerce.Product(),
		Quantity = 1,
		UnitOfMeasure = "kg",
		ValueOfMeasure = 10.5
	};
}
