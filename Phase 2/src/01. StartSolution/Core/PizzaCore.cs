namespace Core;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Mappers;
using Common.Models;
using Core.Contracts;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public class PizzaCore : IPizzaCore
{
	private readonly DatabaseContext databaseContext;

	public PizzaCore(DatabaseContext databaseContext)
		=> (this.databaseContext) = (databaseContext);

	public async Task<PizzaModel> GetAsync(int id)
		=> (await this.databaseContext.Pizzas.FirstOrDefaultAsync(x => x.Id == id)).Map();

	public async Task<IEnumerable<PizzaModel>> GetAllAsync()
		=> (await this.databaseContext.Pizzas.Select(x => x).AsNoTracking().ToListAsync()).Map();

	public async Task<PizzaModel> SaveAsync(PizzaModel pizza)
	{
		var entity = pizza.Map();
		entity.DateCreated = DateTime.UtcNow;
		this.databaseContext.Pizzas.Add(entity);
		await this.databaseContext.SaveChangesAsync();
		pizza.Id = entity.Id;

		return entity.Map();
	}

	public async Task<PizzaModel> UpdateAsync(PizzaModel Pizza)
	{
		var findEntity = await this.databaseContext.Pizzas.FirstOrDefaultAsync(x => x.Id == Pizza.Id);
		if (findEntity == null)
		{
			return null;
		}

		findEntity.Name = !string.IsNullOrEmpty(Pizza.Name) ? Pizza.Name : findEntity.Name;
		findEntity.Description = !string.IsNullOrEmpty(Pizza.Description) ? Pizza.Description : findEntity.Description;
		findEntity.Price = Pizza.Price ?? findEntity.Price;
		this.databaseContext.Pizzas.Update(findEntity);
		await this.databaseContext.SaveChangesAsync();

		return findEntity.Map();
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var findEntity = await this.databaseContext.Pizzas.FirstOrDefaultAsync(x => x.Id == id);
		if (findEntity == null)
		{
			return false;
		}

		this.databaseContext.Pizzas.Remove(findEntity);
		var result = await this.databaseContext.SaveChangesAsync();

		return result == 1;
	}
}