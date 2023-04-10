namespace Core;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Entities;
using Core.Contracts;
using DataAccess;

public class PizzaCore : IPizzaCore
{
	private readonly DatabaseContext databaseContext;

	private readonly IMapper mapper;

	public PizzaCore(DatabaseContext databaseContext, IMapper mapper)
		=> (this.databaseContext, this.mapper) = (databaseContext, mapper);

	public async Task<StockDTO> GetAsync(int id)
		=> this.mapper.Map<StockDTO>(await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == id));

	public async Task<IEnumerable<StockDTO>> GetAllAsync()
	{
		var entities = await this.databaseContext.Stocks.Select(x => x).AsNoTracking().ToListAsync();
		return this.mapper.Map<List<StockDTO>>(entities);
	}

	public async Task<StockDTO> SaveAsync(StockDTO pizza)
	{
		var entity = this.mapper.Map<Stock>(pizza);
		this.databaseContext.Stocks.Add(entity);
		await this.databaseContext.SaveChangesAsync();
		return this.mapper.Map<StockDTO>(entity);
	}

	public async Task<StockDTO> UpdateAsync(StockDTO pizza)
	{
		var findEntity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == pizza.Id);

		findEntity.Name = !string.IsNullOrEmpty(pizza.Name) ? pizza.Name : findEntity.Name;
		findEntity.UnitOfMeasure = !string.IsNullOrEmpty(pizza.UnitOfMeasure) ? pizza.UnitOfMeasure : findEntity.UnitOfMeasure;
		findEntity.ValueOfMeasure = pizza.ValueOfMeasure ?? findEntity.ValueOfMeasure;
		findEntity.Quantity = pizza.Quantity ?? findEntity.Quantity;
		findEntity.ExpiryDate = pizza.ExpiryDate ?? findEntity.ExpiryDate;
		findEntity.Comment = pizza.Comment;
		this.databaseContext.Stocks.Update(findEntity);
		await this.databaseContext.SaveChangesAsync();

		return this.mapper.Map<StockDTO>(findEntity);
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var entity = await this.databaseContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
		this.databaseContext.Stocks.Remove(entity);
		var result = await this.databaseContext.SaveChangesAsync();

		return result == 1;
	}
}