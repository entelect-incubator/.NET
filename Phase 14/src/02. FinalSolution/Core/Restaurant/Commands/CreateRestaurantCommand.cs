namespace Core.Restaurant.Commands;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using Core.Helpers;
using DataAccess;

public sealed class CreateRestaurantCommand : ICommand<Result<RestaurantDTO>>
{
    public RestaurantDTO Data { get; set; }
}

public sealed class CreateRestaurantCommandHandler(DatabaseContext databaseContext) : ICommandHandler<CreateRestaurantCommand, Result<RestaurantDTO>>
{
    public async Task<Result<RestaurantDTO>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Data.ToEntity();
        databaseContext.Restaurants.Add(entity);
        var outcome = await CoreHelper<RestaurantDTO>.Outcome(databaseContext, cancellationToken, request.Data, "Error creating a restaurant");
        if (outcome.Succeeded)
        {
            request.Data.Id = entity.Id;
        }

        return outcome;
    }
}
