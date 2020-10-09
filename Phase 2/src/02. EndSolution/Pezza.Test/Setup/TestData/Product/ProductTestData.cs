namespace Pezza.Test
{
    using System;
    using Bogus;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class ProductTestData
    {
        public static Faker faker = new Faker();

        public static Product Product = new Product()
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
