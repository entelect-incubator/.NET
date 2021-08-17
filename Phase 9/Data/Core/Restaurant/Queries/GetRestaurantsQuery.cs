namespace Pezza.Core.Restaurant.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetRestaurantsQuery : IRequest<ListResult<Common.Entities.Restaurant>>
    {
    }

    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, ListResult<Common.Entities.Restaurant>>
    {
        private readonly IDataAccess<Common.Entities.Restaurant> DataAccess;

        public GetRestaurantsQueryHandler(IDataAccess<Common.Entities.Restaurant> DataAccess) => this.DataAccess = DataAccess;

        public async Task<ListResult<Common.Entities.Restaurant>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var search = await this.DataAccess.GetAllAsync();

            return ListResult<Common.Entities.Restaurant>.Success(search);
        }
    }
}
