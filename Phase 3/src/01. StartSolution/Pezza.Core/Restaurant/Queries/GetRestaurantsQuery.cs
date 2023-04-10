namespace Core.Restaurant.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Models;
using DataAccess;

public class GetRestaurantsQuery : IRequest<ListResult<RestaurantDTO>>
{
}

public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ListResult<RestaurantDTO>>
{
    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetRestaurantsQueryHandler(DatabaseContext databaseContext, IMapper mapper)
        => (this.databaseContext, this.mapper) = (databaseContext, mapper);

    public async Task<ListResult<RestaurantDTO>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var entities = this.databaseContext.Restaurants.Select(x => x).AsNoTracking();

        var count = entities.Count();
        var paged = this.mapper.Map<List<RestaurantDTO>>(await entities.ToListAsync(cancellationToken));

        return ListResult<RestaurantDTO>.Success(paged, count);
    }
}
