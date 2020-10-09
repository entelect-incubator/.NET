namespace Pezza.Test
{
    using System;
    using Bogus;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class RestaurantTestData
    {
        public static Faker faker = new Faker();

        public static Restaurant Restaurant = new Restaurant()
        {
            Name = faker.Company.CompanyName(),
            Description = string.Empty,
            Orders = OrderTestData.Orders(),
            PictureUrl = string.Empty,
            Address = faker.Address.FullAddress(),
            City = faker.Address.City(),
            PostalCode = faker.Address.ZipCode(),
            Province = faker.Address.State(),
            DateCreated = DateTime.Now,
            IsActive = true
        };

        public static RestaurantDTO RestaurantDTO = new RestaurantDTO()
        {
            Name = faker.Company.CompanyName(),
            Description = string.Empty,
            Orders = OrderTestData.OrdersDTO(),
            PictureUrl = string.Empty,
            Address = faker.Address.FullAddress(),
            City = faker.Address.City(),
            PostalCode = faker.Address.ZipCode(),
            Province = faker.Address.State(),
            DateCreated = DateTime.Now,
            IsActive = true
        };
    }

}
