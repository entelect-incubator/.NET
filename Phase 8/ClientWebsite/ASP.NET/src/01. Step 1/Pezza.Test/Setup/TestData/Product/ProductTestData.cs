namespace Test
{
    using System;
    using Bogus;
    using Common.DTO;

    public static class ProductTestData
    {
        public static Faker faker = new Faker();

        public static ProductDTO ProductDTO = new ProductDTO()
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
}