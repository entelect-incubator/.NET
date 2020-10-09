namespace Pezza.Core.Restaurant.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.Common.Models.SearchModels;
    using Pezza.DataAccess.Contracts;

    public class GetRestaurantsQuery : IRequest<ListResult<Common.Entities.Restaurant>>
    {
        public RestaurantSearchModel RestaurantSearchModel { get; set; }
    }

    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ListResult<Common.Entities.Restaurant>>
    {
        private readonly IRestaurantDataAccess dataAcess;

        public GetRestaurantsQueryHandler(IRestaurantDataAccess dataAcess) => this.dataAcess = dataAcess;

        public async Task<ListResult<Common.Entities.Restaurant>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAcess.GetAllAsync(request.RestaurantSearchModel);

            return ListResult<Common.Entities.Restaurant>.Success(search);
        }
    }
}
