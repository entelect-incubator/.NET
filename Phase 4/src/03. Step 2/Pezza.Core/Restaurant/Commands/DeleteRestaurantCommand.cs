namespace Pezza.Core.Restaurant.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public class DeleteRestaurantCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, Result>
    {
        private readonly IDataAccess<RestaurantDTO> dto;

        public DeleteRestaurantCommandHandler(IDataAccess<RestaurantDTO> dto)
            => this.dto = dto;

        public async Task<Result> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dto.DeleteAsync(request.Id);
            return outcome ? Result.Success() : Result.Failure("Error deleting a Restaurant");
        }
    }
}
