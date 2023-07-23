namespace Core.Pizza.Commands;

public class CreatePizzaCommand : IRequest<Result<PizzaModel>>
{
	public CreatePizzaModel? Data { get; set; }
}

public class CreatePizzaCommandHandler(DatabaseContext databaseContext) : IRequestHandler<CreatePizzaCommand, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> Handle(CreatePizzaCommand request, CancellationToken cancellationToken)
	{
		if(request.Data == null)
		{
			return Result<PizzaModel>.Failure("Error");
		}

		var entity = new Common.Entities.Pizza
		{
			Name= request.Data.Name,
			Description= request.Data.Description,
			Price = request.Data.Price,
			DateCreated = DateTime.UtcNow
		};
		databaseContext.Pizzas.Add(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<PizzaModel>.Success(entity.Map()) : Result<PizzaModel>.Failure("Error");
	}
}