namespace Pezza.Core.Restaurant.Commands;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pezza.Common.Models;
using Pezza.Core.Helpers;
using Pezza.DataAccess;

public class DeleteRestaurantCommand : IRequest<Result>
{
    public int Id { get; set; }
}

public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, Result>
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
