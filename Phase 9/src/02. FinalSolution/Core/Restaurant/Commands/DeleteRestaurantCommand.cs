namespace Core.Restaurant.Commands;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using Core.Helpers;
using DataAccess;

public sealed class DeleteRestaurantCommand : ICommand<Result>
{
    public int Id { get; set; }
}

public sealed class DeleteRestaurantCommandHandler : ICommandHandler<DeleteRestaurantCommand, Result>
{
    private readonly DatabaseContext databaseContext;

    public DeleteRestaurantCommandHandler(DatabaseContext databaseContext)
        => this.databaseContext = databaseContext;

    public async Task<Result> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        var findEntity = await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        this.databaseContext.Restaurants.Remove(findEntity);

        return await CoreHelper.Outcome(this.databaseContext, cancellationToken, "Error deleting a restaurant");
    }
}
