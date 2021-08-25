namespace Pezza.Core.Restaurant.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetRestaurantsQuery : IRequest<ListResult<RestaurantDTO>>
    {
        public RestaurantDTO SearchModel { get; set; }
    }

    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ListResult<RestaurantDTO>>
    {
        private readonly IDataAccess<RestaurantDTO> dataAccess;

        public GetRestaurantsQueryHandler(IDataAccess<RestaurantDTO> dataAccess) => this.dataAccess = dataAccess;

        public async Task<ListResult<RestaurantDTO>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
            => await this.dataAccess.GetAllAsync(request.SearchModel);
    }
}
