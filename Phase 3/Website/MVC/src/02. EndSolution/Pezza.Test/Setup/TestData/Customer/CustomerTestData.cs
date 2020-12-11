namespace Pezza.Test
{
    using System;
    using Bogus;
    using Pezza.Common.DTO;
    using Pezza.Common.Entities;

    public static class CustomerTestData
    {
        public static Faker faker = new Faker();

        public static Customer Customer = new Customer()
        {
            Address = faker.Address.FullAddress(),
            Name = faker.Person.FirstName,
            City = faker.Address.City(),
            ContactPerson = faker.Person.FullName,
            Email = faker.Person.Email,
            Phone = faker.Person.Phone,
            Province = faker.Address.State(),
            ZipCode = faker.Address.ZipCode(),
            DateCreated = DateTime.Now
        };

        public static CustomerDTO CustomerDTO = new CustomerDTO()
        {
            Address = faker.Address.FullAddress(),
            City = faker.Address.City(),
            ContactPerson = faker.Person.FullName,
            Email = faker.Person.Email,
            Phone = faker.Person.Phone,
            Province = faker.Address.State(),
            ZipCode = faker.Address.ZipCode(),
            DateCreated = DateTime.Now
        };

        public static CustomerDataDTO CustomerDataDTO = new CustomerDataDTO()
        {
            ContactPerson = faker.Person.FullName,
            Email = faker.Person.Email,
            Phone = faker.Person.Phone,
            AddressBase = new AddressBase
            {
                Address = faker.Address.FullAddress(),
                City = faker.Address.City(),
                Province = faker.Address.State(),
                ZipCode = faker.Address.ZipCode(),
            }
        };
    }

}
