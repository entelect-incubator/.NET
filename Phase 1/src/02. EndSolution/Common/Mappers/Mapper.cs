namespace Common.Mappers;

public static class Mapper
{
	public static PizzaModel Map(this Pizza pizza)
		=> new()
		{
			Id = pizza.Id,
			Name = pizza.Name,
			Description = pizza.Description,
			Price = pizza.Price,
			DateCreated = pizza.DateCreated
		};

	public static Pizza Map(this PizzaModel pizza)
	{
		var entity = new Pizza
		{
			Id = pizza.Id,
			Name = pizza.Name,
			Description = pizza.Description,
			DateCreated = pizza.DateCreated
		};

		if (pizza.Price.HasValue)
		{
			entity.Price = pizza.Price.Value;
		}

		return entity;
	}

	public static IEnumerable<PizzaModel> Map(this List<Pizza> pizzas)
		=> pizzas.Select(x => x.Map());

	public static IEnumerable<Pizza> Map(this List<PizzaModel> pizzas)
		=> pizzas.Select(x => x.Map());
}
