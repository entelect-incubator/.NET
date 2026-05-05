using Common.Models.Todos;

namespace Common.Mappers;

[Mapper]
public static partial class TodoMapper
{
	public static partial Todo Map(this TodoModel model);

	public static partial Todo Map(this CreateTodoModel model);

	public static partial Todo Map(this UpdateTodoModel model);

	public static partial TodoModel Map(this Todo entity);

	public static IEnumerable<TodoModel> Map(this List<Todo> pizzas)
		=> pizzas.Select(x => x.Map());

	public static IEnumerable<Todo> Map(this List<TodoModel> pizzas)
		=> pizzas.Select(x => x.Map());
}
