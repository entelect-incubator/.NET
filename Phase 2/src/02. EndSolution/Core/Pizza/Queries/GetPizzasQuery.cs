namespace Core.Pizza.Queries;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Mappers;
using Common.Models;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetPizzasQuery : IRequest<ListResult<PizzaModel>>
{
	public class GetPizzasQueryHandler : IRequestHandler<GetPizzasQuery, ListResult<PizzaModel>>
	{
		private readonly DatabaseContext databaseContext;

		public GetPizzasQueryHandler(DatabaseContext databaseContext)
			=> this.databaseContext = databaseContext;

		public async Task<ListResult<PizzaModel>> Handle(GetPizzasQuery request, CancellationToken cancellationToken)
		{
			var entities = this.databaseContext.Pizzas.Select(x => x).AsNoTracking();

			var count = entities.Count();
			var paged = await entities.ToListAsync(cancellationToken);

			return ListResult<PizzaModel>.Success(paged.Map(), count);
		}
	}
}