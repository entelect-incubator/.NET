namespace Core.Restaurant.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Mapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public sealed class GetRestaurantQuery : IQuery<Result<RestaurantDTO>>
{
    public int Id { get; set; }
}

public sealed class GetRestaurantQueryHandler(DatabaseContext databaseContext) : IQueryHandler<GetRestaurantQuery, Result<RestaurantDTO>>
{
    public async Task<Result<RestaurantDTO>> Handle(GetRestaurantQuery request, CancellationToken cancellationToken)
    {
        var result = (await databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken))?.ToDto();
        return Result<RestaurantDTO>.Success(result);
    }
}
