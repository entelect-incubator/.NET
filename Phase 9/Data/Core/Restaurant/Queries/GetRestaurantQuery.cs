namespace Pezza.Core.Restaurant.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetRestaurantQuery : IRequest<Result<Common.Entities.Restaurant>>
    {
        public int Id { get; set; }
    }

    public class GetRestaurantQueryHandler : IRequestHandler<GetRestaurantQuery, Result<Common.Entities.Restaurant>>
    {
        private readonly IDataAccess<Common.Entities.Restaurant> DataAccess;

        public GetRestaurantQueryHandler(IDataAccess<Common.Entities.Restaurant> DataAccess) => this.DataAccess = DataAccess;

        public async Task<Result<Common.Entities.Restaurant>> Handle(GetRestaurantQuery request, CancellationToken cancellationToken)
        {
            var search = await this.DataAccess.GetAsync(request.Id);

            return Result<Common.Entities.Restaurant>.Success(search);
        }
    }
}
