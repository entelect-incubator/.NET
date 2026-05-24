namespace Core.Contracts;

public interface IPizzaCore
{
	Task<PizzaModel?> GetAsync(int id);

	Task<IEnumerable<PizzaModel>?> GetAllAsync();

	Task<PizzaModel?> SaveAsync(PizzaModel pizza);

	Task<PizzaModel?> UpdateAsync(PizzaModel pizza);

	Task<bool> DeleteAsync(int id);
}
