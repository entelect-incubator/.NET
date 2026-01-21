namespace Core.Restaurant.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.DTO;
using Common.Extensions;
using Common.Models;
using DataAccess;

public sealed class GetRestaurantsQuery : IQuery<Result<IEnumerable<RestaurantDTO>>>
{
    public RestaurantDTO? Data { get; set; }
}

public sealed class GetRestaurantsQueryHandler(DatabaseContext databaseContext, IMapper mapper, IAppCache cache)
    : IQueryHandler<GetRestaurantsQuery, Result<IEnumerable<RestaurantDTO>>>
{
    private readonly string cacheKey = "RestaurantList";
    private readonly TimeSpan cacheExpiry = new (12, 0, 0);

    public async Task<Result<IEnumerable<RestaurantDTO>>> HandleAsync(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        if (request.Data == null)
        {
            return Result<IEnumerable<RestaurantDTO>>.Failure("Restaurant search data is required");
        }

        var dto = request.Data;

        if (dto.BustCache)
        {
            ClearCache();
        }

        Task<List<RestaurantDTO>> DataDelegate()
        {
            return GetRestaurantData();
        }

        var data = await cache.GetOrAddAsync(cacheKey, DataDelegate, cacheExpiry);

        var orderBy = string.IsNullOrEmpty(dto.OrderBy) ? "DateCreated desc" : dto.OrderBy;
        var orderedData = data.AsQueryable().ApplyPaging(dto.PagingArgs).OrderBy(orderBy);

        var count = data.Count;
        var paged = mapper.Map<List<RestaurantDTO>>(orderedData);

        return Result<IEnumerable<RestaurantDTO>>.Success(paged, count);
    }

    private async Task<List<RestaurantDTO>> GetRestaurantData()
    {
        var entities = await databaseContext.Restaurants.Select(x => x)
            .AsNoTracking()
            .ToListAsync();

        return mapper.Map<List<RestaurantDTO>>(entities);
    }

    private void ClearCache() => cache.Remove(cacheKey);
}
