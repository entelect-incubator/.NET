namespace Test.Setup.TestData.Customer
{
    using System;
    using Bogus;
    using Common.DTO;
    using Common.Models.Base;

    public static class CustomerTestData
    {
        public static Faker faker = new();

        public static CustomerDTO CustomerDTO = new()
        {
            ContactPerson = faker.Person.FullName,
            Email = faker.Person.Email,
            Phone = faker.Person.Phone,
            Address = new AddressBase
            {
                Address = faker.Address.FullAddress(),
                City = faker.Address.City(),
                Province = faker.Address.State(),
                PostalCode = faker.Address.ZipCode(),
            },
            DateCreated = DateTime.Now
        };
    }
}