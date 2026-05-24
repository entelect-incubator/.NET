namespace Core.Restaurant.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Common.DTO;
using Common.Extensions;
using Common.Mapper;
using DataAccess;
using LazyCache;
using Microsoft.EntityFrameworkCore;

public sealed class GetRestaurantsQuery : IQuery<Result<IEnumerable<RestaurantDTO>>>
{
    public RestaurantDTO Data { get; set; }
}

public sealed class GetRestaurantsQueryHandler : IQueryHandler<GetRestaurantsQuery, Result<IEnumerable<RestaurantDTO>>>
{
    private readonly IAppCache cache;

    private readonly string cacheKey = "RestaurantList";

    private readonly TimeSpan cacheExpiry = new(12, 0, 0);

    private readonly DatabaseContext databaseContext;

    public GetRestaurantsQueryHandler(DatabaseContext databaseContext, IAppCache cache)
        => (this.databaseContext, this.cache) = (databaseContext, cache);

    public async Task<Result<IEnumerable<RestaurantDTO>>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data ?? new RestaurantDTO();

        if (dto.BustCache)
        {
            this.ClearCache();
        }

        Task<List<RestaurantDTO>> DataDelegate()
        {
            return this.GetRestaurantData();
        }

        var data = await this.cache.GetOrAddAsync(this.cacheKey, DataDelegate, this.cacheExpiry);

        var orderBy = string.IsNullOrEmpty(dto.OrderBy) ? "DateCreated desc" : dto.OrderBy;
        var orderedData = data.AsQueryable().ApplyPaging(dto.PagingArgs).OrderBy(orderBy).ToList();

        var count = data.Count;

        return Result<IEnumerable<RestaurantDTO>>.Success(orderedData, count);
    }

    private async Task<List<RestaurantDTO>> GetRestaurantData()
    {
        var entities = await this.databaseContext.Restaurants.Select(x => x)
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(x => x.ToDto()).ToList();
    }

    private void ClearCache() => this.cache.Remove(this.cacheKey);
}
