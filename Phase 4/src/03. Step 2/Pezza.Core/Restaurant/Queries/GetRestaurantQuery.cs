namespace Pezza.Core.Restaurant.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class GetRestaurantQuery : IRequest<Result<RestaurantDTO>>
    {
        public int Id { get; set; }
    }

    public class GetRestaurantQueryHandler : IRequestHandler<GetRestaurantQuery, Result<RestaurantDTO>>
    {
        private readonly IDataAccess<RestaurantDTO> dataAccess;

        public GetRestaurantQueryHandler(IDataAccess<RestaurantDTO> dataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<RestaurantDTO>> Handle(GetRestaurantQuery request, CancellationToken cancellationToken)
        {
            var search = await this.dataAccess.GetAsync(request.Id);
            return Result<RestaurantDTO>.Success(search);
        }
    }
}
