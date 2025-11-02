namespace Test;

using System;
using Bogus;
using Common.DTO;

public static class PizzaTestData
{
    public static Faker faker = new Faker();

    public static PizzaModel PizzaModel = new PizzaModel()
    {
        Comment = faker.Lorem.Sentence(),
        ExpiryDate = DateTime.Now.AddMonths(1),
        Name = faker.Commerce.Product(),
        Quantity = 1,
        UnitOfMeasure = "kg",
        ValueOfMeasure = 10.5
    };
}