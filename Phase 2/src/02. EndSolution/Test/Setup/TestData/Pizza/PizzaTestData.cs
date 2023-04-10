namespace Test.Setup.TestData.Pizza;

using Bogus;
using Common.Entities;
using Common.Models;

public static class CustomerTestData
{
	public static Faker faker = new("en_ZA");

	public static Customer Customer = new()
	{
		Id = 1,
		Name = faker.Person.FullName,
		Address = faker.Address.FullAddress(),
		Cellphone = faker.Phone.PhoneNumber(),
		Email = faker.Person.Email,
		DateCreated = DateTime.Now,
	};

	public static CustomerModel CustomerModel = new()
	{
		Id = 1,
		Name = faker.Person.FullName,
		Address = faker.Address.FullAddress(),
		Cellphone = faker.Phone.PhoneNumber(),
		Email = faker.Person.Email,
		DateCreated = DateTime.Now,
	};
}
