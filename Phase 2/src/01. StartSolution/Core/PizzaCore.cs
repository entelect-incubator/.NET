namespace Core;

public class PizzaCore(DatabaseContext databaseContext) : IPizzaCore
{
	public async Task<PizzaModel?> GetAsync(int id)
	{
		var entity = await databaseContext.Pizzas.FirstOrDefaultAsync(x => x.Id == id);
		if(entity == null)
		{
			return null;
		}

		return entity.Map();
	}

	public async Task<IEnumerable<PizzaModel>?> GetAllAsync()
	{
		var entities = await databaseContext.Pizzas.Select(x => x).AsNoTracking().ToListAsync();
		if (entities.Count == 0)
		{
			return null;
		}

		return entities.Map();
	}

		public async Task<PizzaModel?> SaveAsync(PizzaModel pizza)
	{
		if(pizza == null)
		{
			return null;
		}

		var entity = pizza.Map();
		entity.DateCreated = DateTime.UtcNow;
		databaseContext.Pizzas.Add(entity);
		await databaseContext.SaveChangesAsync();
		pizza.Id = entity.Id;

		return entity.Map();
	}

	public async Task<PizzaModel?> UpdateAsync(PizzaModel Pizza)
	{
		var findEntity = await databaseContext.Pizzas.FirstOrDefaultAsync(x => x.Id == Pizza.Id);
		if (findEntity == null)
		{
			return null;
		}

		findEntity.Name = !string.IsNullOrEmpty(Pizza.Name) ? Pizza.Name : findEntity.Name;
		findEntity.Description = !string.IsNullOrEmpty(Pizza.Description) ? Pizza.Description : findEntity.Description;
		findEntity.Price = Pizza.Price ?? findEntity.Price;
		databaseContext.Pizzas.Update(findEntity);
		await databaseContext.SaveChangesAsync();

		return findEntity.Map();
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var result = await databaseContext.Pizzas
			.Where(e => e.Id == id)
			.ExecuteDeleteAsync();

		return result == 1;
	}
}