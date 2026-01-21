namespace Core.Pizza.Commands;

public sealed class CreatePizzaCommand : ICommand<Result<PizzaModel>>
{
	public required CreatePizzaModel Data { get; set; }
}

public sealed class CreatePizzaCommandHandler(DatabaseContext databaseContext) : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
{
	public async Task<Result<PizzaModel>> Handle(CreatePizzaCommand request, CancellationToken cancellationToken)
	{
		var entity = new Common.Entities.Pizza
		{
			Name = request.Data.Name,
			Description = request.Data.Description,
			Price = request.Data.Price,
			DateCreated = DateTime.UtcNow
		};
		databaseContext.Pizzas.Add(entity);
		var result = await databaseContext.SaveChangesAsync(cancellationToken);

		return result > 0 ? Result<PizzaModel>.Success(entity.Map()) : Result<PizzaModel>.Failure("Error");
	}
}