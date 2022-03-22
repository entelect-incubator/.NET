namespace Pezza.Core.Restaurant.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateRestaurantCommand : IRequest<Result<RestaurantDTO>>
    {
        public RestaurantDTO Data { get; set; }
    }

    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Result<RestaurantDTO>>
    {
        private readonly IDataAccess<RestaurantDTO> dataAccess;

        public CreateRestaurantCommandHandler(IDataAccess<RestaurantDTO> dataAccess) => this.dataAccess = dataAccess;

        public async Task<Result<RestaurantDTO>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAccess.SaveAsync(request.Data);
            return (outcome != null) ? Result<RestaurantDTO>.Success(outcome) : Result<RestaurantDTO>.Failure("Error adding a Restaurant");
        }
    }
}
