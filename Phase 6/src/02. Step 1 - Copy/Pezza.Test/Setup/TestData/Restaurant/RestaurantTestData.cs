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
            PictureUrl = string.Empty,
            Address = faker.Address.FullAddress(),
            City = faker.Address.City(),
            PostalCode = faker.Address.ZipCode(),
            Province = faker.Address.State(),
            DateCreated = DateTime.Now,
            Id = 1,
            IsActive = true
        };

        public static RestaurantDTO RestaurantDTO = new RestaurantDTO()
        {
            Name = faker.Company.CompanyName(),
            Description = string.Empty,
            PictureUrl = string.Empty,
            Address = faker.Address.FullAddress(),
            City = faker.Address.City(),
            PostalCode = faker.Address.ZipCode(),
            Province = faker.Address.State(),
            DateCreated = DateTime.Now,
            Id = 1,
            IsActive = true
        };

        public static RestaurantDataDTO RestaurantDataDTO = new RestaurantDataDTO()
        {
            Name = faker.Company.CompanyName(),
            Description = string.Empty,
            PictureUrl = string.Empty,
            Address = new AddressBase
            {
                Address = faker.Address.FullAddress(),
                City = faker.Address.City(),
                ZipCode = faker.Address.ZipCode(),
                Province = faker.Address.State(),
            },
            IsActive = true
        };
    }

}
