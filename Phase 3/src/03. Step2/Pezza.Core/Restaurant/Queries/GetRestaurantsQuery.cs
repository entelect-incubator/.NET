namespace Pezza.Core.Restaurant.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Pezza.Common.DTO;
    using Pezza.Common.Extensions;
    using Pezza.Common.Models;
    using Pezza.DataAccess;

    public class GetRestaurantsQuery : IRequest<ListResult<RestaurantDTO>>
    {
        public RestaurantDTO Data { get; set; }
    }

    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ListResult<RestaurantDTO>>
    {
        private readonly DatabaseContext databaseContext;

        private readonly IMapper mapper;

        public GetRestaurantsQueryHandler(DatabaseContext databaseContext, IMapper mapper)
            => (this.databaseContext, this.mapper) = (databaseContext, mapper);

        public async Task<ListResult<RestaurantDTO>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var entities = this.databaseContext.Restaurants.Select(x => x).AsNoTracking();

            var count = entities.Count();
            var orderBy = string.IsNullOrEmpty(dto.OrderBy) ? "DateCreated desc" : dto.OrderBy;
            var paged = this.mapper.Map<List<RestaurantDTO>>(await entities.AsQueryable().ApplyPaging(dto.PagingArgs).OrderBy(orderBy).ToListAsync(cancellationToken));

            return ListResult<RestaurantDTO>.Success(paged, count);
        }
    }
}
