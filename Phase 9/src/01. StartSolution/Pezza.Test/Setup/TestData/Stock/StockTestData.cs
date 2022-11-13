namespace Pezza.Test;

using System;
using Bogus;
using Pezza.Common.DTO;

public static class StockTestData
{
    public static Faker faker = new Faker();

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