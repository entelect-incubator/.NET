namespace Test.Setup.TestData.Product;

using System;
using Bogus;
using Common.DTO;

public static class ProductTestData
{
    public static Faker faker = new();

    public static ProductDTO ProductDTO = new()
    {
        Name = faker.Commerce.Product(),
        Description = string.Empty,
        Price = faker.Finance.Amount(),
        PictureUrl = string.Empty,
        OfferEndDate = null,
        OfferPrice = null,
        Special = false,
        DateCreated = DateTime.Now,
        IsActive = true
    };
}