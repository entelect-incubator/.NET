namespace Core.Pizza.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.Mappers;
using Common.Models;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdatePizzaCommand : IRequest<Result<PizzaModel>>
{
	public int? Id { get; set; }

	public UpdatePizzaModel? Data { get; set; }

	public class UpdatePizzaCommandHandler : IRequestHandler<UpdatePizzaCommand, Result<PizzaModel>>
	{
		private readonly DatabaseContext databaseContext;

		public UpdatePizzaCommandHandler(DatabaseContext databaseContext)
			=> this.databaseContext = databaseContext;

		public async Task<Result<PizzaModel>> Handle(UpdatePizzaCommand request, CancellationToken cancellationToken)
		{
			if (request.Data == null || request.Id == null)
			{
				return Result<PizzaModel>.Failure("Error updating a Pizza");
			}

			var model = request.Data;
			var query = EF.CompileAsyncQuery((DatabaseContext db, int id) => db.Pizzas.FirstOrDefault(c => c.Id == id));
			var findEntity = await query(this.databaseContext, request.Id.Value);
			if (findEntity == null)
			{
				return Result<PizzaModel>.Failure("Error finding a Pizza");
			}

			findEntity.Name = !string.IsNullOrEmpty(model?.Name) ? model?.Name : findEntity.Name;
			findEntity.Description = !string.IsNullOrEmpty(model?.Description) ? model?.Description : findEntity.Description;
			findEntity.Price = model.Price.HasValue ? model.Price.Value : findEntity.Price;

			var outcome = this.databaseContext.Pizzas.Update(findEntity);
			var result = await this.databaseContext.SaveChangesAsync();

			return result > 0 ? Result<PizzaModel>.Success(findEntity.Map()) : Result<PizzaModel>.Failure("Error updating a Pizza");
		}
	}
}