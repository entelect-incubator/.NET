namespace Core.Pizza.Commands;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using DataAccess;

public class DeletePizzaCommand : IRequest<Result>
{
	public int? Id { get; set; }

	public class DeletePizzaCommandHandler : IRequestHandler<DeletePizzaCommand, Result>
	{
		private readonly DatabaseContext databaseContext;

		public DeletePizzaCommandHandler(DatabaseContext databaseContext)
			=> this.databaseContext = databaseContext;

		public async Task<Result> Handle(DeletePizzaCommand request, CancellationToken cancellationToken)
		{
			if (request.Id == null)
			{
				return Result.Failure("Error deleting a Pizza");
			}

			var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
			var findEntity = await query(this.databaseContext, request.Id.Value);
			if (findEntity == null)
			{
				return Result.Failure("Pizza not found");
			}

			this.databaseContext.Pizzas.Remove(findEntity);
			var result = await this.databaseContext.SaveChangesAsync();

			return result > 0 ? Result.Success() : Result.Failure("Error deleting a Pizza");
		}
	}
}