namespace Pezza.Core.Restaurant.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetRestaurantsQuery : IRequest<ListResult<RestaurantDTO>>
    {
        public RestaurantDataDTO SearchModel { get; set; }
    }

    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ListResult<RestaurantDTO>>
    {
        private readonly IDataAccess<Common.Entities.Restaurant> dataAcess;

        public GetRestaurantsQueryHandler(IDataAccess<Common.Entities.Restaurant> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<RestaurantDTO>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.SearchModel);

            return ListResult<RestaurantDTO>.Success(search.Data.Map(), search.Count);
        }
    }
}
