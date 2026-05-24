namespace Test.Setup.TestData.Customer;

public static class CustomerTestData
{
	public static Faker faker = new("en_ZA");

	public static Common.Entities.Customer Customer = new()
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