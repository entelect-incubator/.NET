namespace Core.Pizza.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.Mappers;
using Common.Models;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetPizzaQuery : IRequest<Result<PizzaModel>>
{
	public int Id { get; set; }

	public class GetPizzaQueryHandler : IRequestHandler<GetPizzaQuery, Result<PizzaModel>>
	{
		private readonly DatabaseContext databaseContext;

		public GetPizzaQueryHandler(DatabaseContext databaseContext)
			=> this.databaseContext = databaseContext;

		public async Task<Result<PizzaModel>> Handle(GetPizzaQuery request, CancellationToken cancellationToken)
		{
			var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
			return Result<PizzaModel>.Success((await query(this.databaseContext, request.Id)).Map());
		}
	}
}