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
    }

    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ListResult<RestaurantDTO>>
    {
        private readonly IDataAccess<RestaurantDTO> dataAcess;

        public GetRestaurantsQueryHandler(IDataAccess<RestaurantDTO> dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<RestaurantDTO>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync();
            return ListResult<RestaurantDTO>.Success(search, search.Count);
        }
    }
}
