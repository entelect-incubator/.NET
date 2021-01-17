namespace Pezza.Core.Restaurant.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Entities;
    using MediatR;
    using Pezza.Common.DTO;
    using Pezza.Common.Mapping;
    using Pezza.Common.Models;
    using Pezza.DataAccess.Contracts;

    public partial class CreateRestaurantCommand : IRequest<Result<Restaurant>>
    {
        public RestaurantDataDTO Data { get; set; }
    }

    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Result<Restaurant>>
    {
        private readonly IDataAccess<Restaurant> dataAcess;

        public CreateRestaurantCommandHandler(IDataAccess<Restaurant> dataAcess) => this.dataAcess = dataAcess;

        public async Task<Result<Restaurant>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var outcome = await this.dataAcess.SaveAsync(request.Data.Map());

            return (outcome != null) ? Result<Restaurant>.Success(outcome) : Result<Restaurant>.Failure("Error adding a Restaurant");
        }
    }
}
