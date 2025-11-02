namespace Core.Restaurant.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LazyCache;

using Microsoft.EntityFrameworkCore;
using Pezza.Pezza.Common.DTO;
using Pezza.Common.Extensions;
using Pezza.Pezza.Common.Models;
using DataAccess;

public sealed class GetRestaurantsQuery : ICommand<ListResult<RestaurantDTO>>
{
    public RestaurantDTO Data { get; set; }
}

public sealed class GetRestaurantsQueryHandler : ICommandHandler<GetRestaurantsQuery, ListResult<RestaurantDTO>>
{
    private readonly IAppCache cache;

    private readonly string cacheKey = "RestaurantList";

    private readonly TimeSpan cacheExpiry = new (12, 0, 0);

    private readonly DatabaseContext databaseContext;

    private readonly IMapper mapper;

    public GetRestaurantsQueryHandler(DatabaseContext databaseContext, IMapper mapper, IAppCache cache)
        => (this.databaseContext, this.mapper, this.cache) = (databaseContext, mapper, cache);

    public async Task<ListResult<RestaurantDTO>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

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
        var orderedData = data.AsQueryable().ApplyPaging(dto.PagingArgs).OrderBy(orderBy);

        var count = data.Count;
        var paged = this.mapper.Map<List<RestaurantDTO>>(orderedData);

        return ListResult<RestaurantDTO>.Success(paged, count);
    }

    private async Task<List<RestaurantDTO>> GetRestaurantData()
    {
        var entities = await this.databaseContext.Restaurants.Select(x => x)
            .AsNoTracking()
            .ToListAsync();

        return this.mapper.Map<List<RestaurantDTO>>(entities);
    }

    private void ClearCache() => this.cache.Remove(this.cacheKey);
}

