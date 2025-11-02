namespace Core.Restaurant.Queries;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Pezza.Pezza.Common.DTO;
using Pezza.Pezza.Common.Models;
using DataAccess;

public sealed class GetRestaurantQuery : ICommand<Result<RestaurantDTO>>
{
    public int Id { get; set; }
}

public sealed class GetRestaurantQueryHandler : ICommandHandler<GetRestaurantQuery, Result<RestaurantDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetRestaurantQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<Result<RestaurantDTO>> Handle(GetRestaurantQuery request, CancellationToken cancellationToken)
    {
        var result = this.mapper.Map<RestaurantDTO>(await this.databaseContext.Restaurants.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));
        return Result<RestaurantDTO>.Success(result);
    }
}

