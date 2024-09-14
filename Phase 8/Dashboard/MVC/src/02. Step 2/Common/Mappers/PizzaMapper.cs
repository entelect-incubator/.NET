namespace Common.Mappers;

public static class PizzaMapper
{
	public static PizzaModel Map(this Pizza entity)
		=> new()
		{
			Id = entity.Id,
			Name = entity.Name,
			Description = entity.Description,
			Price = entity.Price,
			DateCreated = entity.DateCreated
		};

	public static Pizza Map(this PizzaModel model)
	{
		var entity = new Pizza
		{
			Id = model.Id,
			Name = model.Name,
			Description = model.Description,
			DateCreated = model.DateCreated
		};

		if (model.Price.HasValue)
		{
			entity.Price = model.Price.Value;
		}

		return entity;
	}

	public static List<PizzaModel> Map(this List<Pizza> entities)
		=> entities.Select(x => x.Map()).ToList();

	public static List<Pizza> Map(this List<PizzaModel> models)
		=> models.Select(x => x.Map()).ToList();
}
