using Common.Models.Todos;

namespace Test.Setup.TestData.Pizza;

public static class TodoTestData
{
	public static Faker faker = new();

	public static Todo Todo = new()
	{
		Id = 1,
		Task = faker.Random.Word(),
		IsCompleted = false,
		DateCreated = DateTime.UtcNow,
		SessionId = Guid.NewGuid(),	
	};

	public static TodoModel TodoModel = new()
	{
		Id = 1,
		Task = faker.Random.Word(),
		IsCompleted = false,
		DateCreated = DateTime.UtcNow,
		SessionId = Guid.NewGuid(),
	};

	public static CreateTodoModel CreateTodoModel = new()
	{
		Task = faker.Random.Word(),
		IsCompleted = false,
		SessionId = Guid.NewGuid(),
	};

	public static UpdateTodoModel UpdateTodoModel = new()
	{
		Task = faker.Random.Word(),
	};
}
