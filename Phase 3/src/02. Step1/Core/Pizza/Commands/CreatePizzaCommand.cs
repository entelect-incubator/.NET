namespace Core.Pizza.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.Mappers;
using Common.Models;
using DataAccess;
using MediatR;

public class CreatePizzaCommand : IRequest<Result<PizzaModel>>
{
	public CreatePizzaModel? Data { get; set; }

	public class CreatePizzaCommandHandler : IRequestHandler<CreatePizzaCommand, Result<PizzaModel>>
	{
		private readonly DatabaseContext databaseContext;

		public CreatePizzaCommandHandler(DatabaseContext databaseContext)
			=> this.databaseContext = databaseContext;

		public async Task<Result<PizzaModel>> Handle(CreatePizzaCommand request, CancellationToken cancellationToken)
		{
			if(request.Data == null)
			{
				return Result<PizzaModel>.Failure("Error creatiing a Pizza");
			}

			var entity = new Common.Entities.Pizza
			{
				Name= request.Data.Name,
				Description= request.Data.Description,
				Price = request.Data.Price,
				DateCreated = DateTime.UtcNow
			};
			this.databaseContext.Pizzas.Add(entity);
			var result = await this.databaseContext.SaveChangesAsync();

			return result > 0 ? Result<PizzaModel>.Success(entity.Map()) : Result<PizzaModel>.Failure("Error creatiing a Pizza");
		}
	}
}