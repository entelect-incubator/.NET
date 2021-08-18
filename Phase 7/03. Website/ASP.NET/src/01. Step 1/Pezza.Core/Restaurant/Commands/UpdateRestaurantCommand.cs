namespace Pezza.Core.Restaurant.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class UpdateRestaurantCommand : IRequest<Result<RestaurantDTO>>
    {
        public RestaurantDTO Data { get; set; }
    }

    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, Result<RestaurantDTO>>
    {
        private readonly IDataAccess<RestaurantDTO> DataAccess;

        public UpdateRestaurantCommandHandler(IDataAccess<RestaurantDTO> DataAccess) => this.DataAccess = DataAccess;

        public async Task<Result<RestaurantDTO>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.DataAccess.UpdateAsync(request.Data);
            return (outcome != null) ? Result<RestaurantDTO>.Success(outcome) : Result<RestaurantDTO>.Failure("Error updating a Restaurant");
        }
    }
}