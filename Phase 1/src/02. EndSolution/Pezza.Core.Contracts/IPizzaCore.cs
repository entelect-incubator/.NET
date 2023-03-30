namespace Pezza.Core.Contracts;

using System.Collections.Generic;
using System.Threading.Tasks;
using Pezza.Common.Models;

public interface IPizzaCore
{
	Task<PizzaModel> GetAsync(int id);

	Task<IEnumerable<PizzaModel>> GetAllAsync();

	Task<PizzaModel> UpdateAsync(PizzaModel pizza);

	Task<PizzaModel> SaveAsync(PizzaModel pizza);

	Task<bool> DeleteAsync(int id);
}
