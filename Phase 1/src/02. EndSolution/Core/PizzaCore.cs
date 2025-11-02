namespace Core;

public class PizzaCore(DatabaseContext databaseContext) : IPizzaCore
{
	public async Task<PizzaModel?> GetAsync(int id)
	{
		var entity = await databaseContext.Pizzas.FirstOrDefaultAsync(x => x.Id == id);
		if(entity is null)
		{
			return null;
		}

		return entity.Map();
	}

	public async Task<IEnumerable<PizzaModel>?> GetAllAsync()
	{
		var entities = await databaseContext.Pizzas.Select(x => x).AsNoTracking().ToListAsync();
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
		if (findEntity is null)
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
		var entity = await databaseContext.Pizzas.FirstOrDefaultAsync(x => x.Id == id);
		if (entity is null)
		{
			return false;
		}

		databaseContext.Pizzas.Remove(entity);
		await databaseContext.SaveChangesAsync();

		return true;
	}
}