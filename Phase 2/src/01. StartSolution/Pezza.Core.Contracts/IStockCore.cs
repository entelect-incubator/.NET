namespace Core.Contracts;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;

public interface IPizzaCore
{
	Task<StockDTO> GetAsync(int id);

	Task<IEnumerable<StockDTO>> GetAllAsync();

	Task<StockDTO> UpdateAsync(StockDTO pizza);

	Task<StockDTO> SaveAsync(StockDTO pizza);

	Task<bool> DeleteAsync(int id);
}
