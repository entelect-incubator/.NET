namespace Test.Setup.TestData.Customer;

public static class CustomerTestData
{
	public static Faker faker = new();

	public static CustomerModel Customer = new()
	{
		Id = 1,
		Name = faker.Name.FullName(),
		Address = faker.Address.StreetAddress(),
		Email = faker.Internet.Email(),
		Cellphone = faker.Phone.PhoneNumber("072#######"),
		DateCreated = DateTime.Now,
	};

	public static CustomerModel CustomerModel = new()
	{
		Id = 1,
		Name = faker.Name.FullName(),
		Address = faker.Address.StreetAddress(),
		Email = faker.Internet.Email(),
		Cellphone = faker.Phone.PhoneNumber("072#######"),
		DateCreated = DateTime.Now,
	};
}
