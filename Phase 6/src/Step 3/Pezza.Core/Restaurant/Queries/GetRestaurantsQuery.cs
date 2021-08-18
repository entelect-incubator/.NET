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
        public RestaurantDTO dto;
    }

    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ListResult<RestaurantDTO>>
    {
        private readonly IDataAccess<RestaurantDTO> dto;

        public GetRestaurantsQueryHandler(IDataAccess<RestaurantDTO> dto) => this.dto = dto;

        public async Task<ListResult<RestaurantDTO>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dto.GetAllAsync(request.dto);
            return search;
        }
    }
}
