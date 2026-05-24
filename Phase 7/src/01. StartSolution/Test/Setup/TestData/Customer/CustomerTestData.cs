namespace Test.Setup.TestData.Customer;

public static class CustomerTestData
{
	public static Faker faker = new();

	public static CustomerModel CustomerModel = new()
	{
		Id = 1,
		Name = faker.Person.FullName,
		Address = faker.Address.FullAddress(),
		Email = faker.Person.Email,
		Cellphone = faker.Person.Phone,
		DateCreated = DateTime.Now
	};
}
